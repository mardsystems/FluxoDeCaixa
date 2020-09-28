using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : ControllerBase
    {
        private readonly ILogger<LancamentosController> logger;

        private readonly Dictionary<TipoDeLancamento, string> queueBy;

        public LancamentosController(ILogger<LancamentosController> logger)
        {
            this.logger = logger;

            queueBy = new Dictionary<TipoDeLancamento, string>();

            queueBy.Add(TipoDeLancamento.Pagamento, "pagamentos");

            queueBy.Add(TipoDeLancamento.Recebimento, "recebimentos");
        }

        // POST api/<LancamentosController>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ComandoDeLancamentoFinanceiro comando)
        {
            var protocolo = new Protocolo();

            logger.LogDebug($"Protocolo: {protocolo.Id}");

            comando.AnexaProtocolo(protocolo);

            //

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueBy[TipoDeLancamento.Pagamento],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    channel.QueueDeclare(
                        queue: queueBy[TipoDeLancamento.Recebimento],
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var content = JsonConvert.SerializeObject(comando);

                    var body = Encoding.UTF8.GetBytes(content);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueBy[comando.TipoDeLancamento],
                        basicProperties: null,
                        body: body
                    );
                }
            }

            return Ok(protocolo);
        }
    }
}
