using FluxoDeCaixa.Modulos.Lancamentos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class ConsolidacaoRabbitMQService :
        IRequestHandler<ComandoParaIniciarConsolidacao>,
        IDisposable
    {
        private readonly IConnection connection;

        private readonly IModel channel;

        private readonly IServiceScope consumerScope;

        public ConsolidacaoRabbitMQService(IServiceScopeFactory scopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "message_broker" };

            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "lancamentos-financeiros-processados",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            consumerScope = scopeFactory.CreateScope();
        }

        public Task<Unit> Handle(ComandoParaIniciarConsolidacao request, CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();

                var content = Encoding.UTF8.GetString(body);

                var evento = JsonConvert.DeserializeObject<EventoDeLancamentoFinanceiroProcessado>(content);

                Console.WriteLine(" [x] EventoDeLancamentoFinanceiroProcessado Received {0}", content);

                var mediator = consumerScope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Publish(evento);

                channel.BasicAck(args.DeliveryTag, false);
            };

            channel.BasicConsume(
                queue: "lancamentos-financeiros-processados",
                autoAck: false,
                consumer: consumer
            );

            return Unit.Task;
        }

        public void Dispose()
        {
            channel.Dispose();

            connection.Dispose();

            consumerScope.Dispose();
        }
    }
}
