using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class LancamentosService : BackgroundService
    {
        private IConnection connection;

        private IModel channel;

        private IMediator mediator;

        public LancamentosService(IMediator mediator)
        {
            this.mediator = mediator;

            var factory = new ConnectionFactory() { HostName = "localhost" };

            connection = factory.CreateConnection();

            channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "pagamentos",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();

                var content = Encoding.UTF8.GetString(body);

                var solicitacao = JsonConvert.DeserializeObject<ComandoDePagamento>(content);

                Console.WriteLine(" [x] Received {0}", content);

                await mediator.Send(solicitacao);
            };

            channel.BasicConsume(
                queue: "pagamentos",
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Dispose();

            connection.Dispose();

            base.Dispose();
        }
    }
}
