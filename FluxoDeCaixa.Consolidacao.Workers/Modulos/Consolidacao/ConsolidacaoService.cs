using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
    public class ConsolidacaoService : BackgroundService
    {
        private readonly IConnection connection;

        private readonly IModel channel;

        private readonly IServiceScope consumerScope;

        public ConsolidacaoService(IServiceScopeFactory scopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                var body = args.Body.ToArray();

                var content = Encoding.UTF8.GetString(body);

                var evento = JsonConvert.DeserializeObject<EventoDeLancamentoFinanceiroProcessado>(content);

                Console.WriteLine(" [x] Received {0}", content);

                var mediator = consumerScope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Publish(evento);
            };

            channel.BasicConsume(
                queue: "lancamentos-financeiros-processados",
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Dispose();

            connection.Dispose();

            consumerScope.Dispose();

            base.Dispose();
        }
    }
}
