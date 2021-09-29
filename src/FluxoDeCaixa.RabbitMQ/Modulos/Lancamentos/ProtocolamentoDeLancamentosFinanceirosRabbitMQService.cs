using MediatR;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    /// <summary>
    /// Serviço RabbitMQ para protocolar lançamentos financeiros.
    /// </summary>
    public class ProtocolamentoDeLancamentosFinanceirosRabbitMQService : IRequestHandler<ComandoDeLancamentoFinanceiro>
    {
        private readonly ConnectionFactory connectionFactory;

        private readonly Dictionary<TipoDeLancamento, string> queueBy;

        public ProtocolamentoDeLancamentosFinanceirosRabbitMQService(IConfiguration configuration)
        {
            connectionFactory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ_HostName"]
            };

            //

            queueBy = new Dictionary<TipoDeLancamento, string>();

            queueBy.Add(TipoDeLancamento.Pagamento, "pagamentos");

            queueBy.Add(TipoDeLancamento.Recebimento, "recebimentos");
        }

        public async Task<Unit> Handle(ComandoDeLancamentoFinanceiro comando, CancellationToken cancellationToken)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueBy[comando.TipoDeLancamento],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var content = JsonSerializer.Serialize(comando);

                    var body = Encoding.UTF8.GetBytes(content);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueBy[comando.TipoDeLancamento],
                        basicProperties: null,
                        body: body
                    );

                    Console.WriteLine(" [x] ComandoDeLancamentoFinanceiro Published {0}", content);
                }
            }

            return await Unit.Task;
        }
    }
}
