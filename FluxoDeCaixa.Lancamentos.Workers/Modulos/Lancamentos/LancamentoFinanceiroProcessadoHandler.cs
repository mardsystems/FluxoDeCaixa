using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class LancamentoFinanceiroProcessadoHandler : INotificationHandler<EventoDeLancamentoFinanceiroProcessado>
    {
        public Task Handle(EventoDeLancamentoFinanceiroProcessado evento, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "message_broker" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "lancamentos-financeiros-processados",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var content = JsonConvert.SerializeObject(evento);

                    var body = Encoding.UTF8.GetBytes(content);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "lancamentos-financeiros-processados",
                        basicProperties: null,
                        body: body
                    );
                }
            }

            return Task.CompletedTask;
        }
    }
}
