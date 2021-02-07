using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    /// <summary>
    /// Serviço RabbitMQ para processar lançamentos financeiros.
    /// </summary>
    public class ProcessamentoDeLancamentosFinanceirosRabbitMQService : IRequestHandler<ComandoParaProcessarLancamentosFinanceiros>, IDisposable
    {
        private readonly Dictionary<TipoDeLancamento, string> queueBy;

        private readonly IServiceScopeFactory scopeFactory;

        private readonly ConnectionFactory factory;

        private IConnection connection;

        private IModel pagamentosChannel;

        private IModel recebimentosChannel;

        private IServiceScope consumerScope;

        public ProcessamentoDeLancamentosFinanceirosRabbitMQService(IServiceScopeFactory scopeFactory)
        {
            queueBy = new Dictionary<TipoDeLancamento, string>();

            queueBy.Add(TipoDeLancamento.Pagamento, "pagamentos");

            queueBy.Add(TipoDeLancamento.Recebimento, "recebimentos");

            this.scopeFactory = scopeFactory;

            factory = new ConnectionFactory() { HostName = "message_broker" };
        }

        public async Task<Unit> Handle(ComandoParaProcessarLancamentosFinanceiros comando, CancellationToken cancellationToken)
        {
            connection = factory.CreateConnection();

            pagamentosChannel = connection.CreateModel();

            pagamentosChannel.QueueDeclare(
                queue: queueBy[TipoDeLancamento.Pagamento],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            recebimentosChannel = connection.CreateModel();

            recebimentosChannel.QueueDeclare(
                queue: queueBy[TipoDeLancamento.Recebimento],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            consumerScope = scopeFactory.CreateScope();

            //

            var pagamentosConsumer = new EventingBasicConsumer(pagamentosChannel);

            pagamentosConsumer.Received += Consumer_Received;

            pagamentosChannel.BasicConsume(
                queue: queueBy[TipoDeLancamento.Pagamento],
                autoAck: false,
                consumer: pagamentosConsumer
            );

            var recebimentosConsumer = new EventingBasicConsumer(recebimentosChannel);

            recebimentosConsumer.Received += Consumer_Received;

            recebimentosChannel.BasicConsume(
                queue: queueBy[TipoDeLancamento.Recebimento],
                autoAck: false,
                consumer: recebimentosConsumer
            );

            return await Unit.Task;
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();

            var content = Encoding.UTF8.GetString(body);

            var comando = JsonConvert.DeserializeObject<ComandoDeLancamentoFinanceiro>(content);

            Console.WriteLine(" [x] ComandoDeLancamentoFinanceiro Received {0}", content);

            var mediator = consumerScope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(comando);

            if (comando.TipoDeLancamento == TipoDeLancamento.Pagamento)
            {
                pagamentosChannel.BasicAck(args.DeliveryTag, false);
            }
            else
            {
                recebimentosChannel.BasicAck(args.DeliveryTag, false);
            }
        }

        public void Dispose()
        {
            if (pagamentosChannel != null)
            {
                pagamentosChannel.Dispose();

                connection.Dispose();

                consumerScope.Dispose();
            }
        }
    }
}
