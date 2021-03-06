﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    public class LancamentosService : BackgroundService
    {
        private readonly IConnection connection;

        private readonly IModel pagamentosChannel;

        private readonly IModel recebimentosChannel;

        private readonly IServiceScope consumerScope;

        private readonly Dictionary<TipoDeLancamento, string> queueBy;

        public LancamentosService(IServiceScopeFactory scopeFactory)
        {
            var factory = new ConnectionFactory() { HostName = "message_broker" };

            connection = factory.CreateConnection();

            queueBy = new Dictionary<TipoDeLancamento, string>();

            queueBy.Add(TipoDeLancamento.Pagamento, "pagamentos");

            queueBy.Add(TipoDeLancamento.Recebimento, "recebimentos");

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
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

            return Task.CompletedTask;
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

        public override void Dispose()
        {
            pagamentosChannel.Dispose();

            connection.Dispose();

            consumerScope.Dispose();

            base.Dispose();
        }
    }
}
