using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Route("api/[controller]")]
    [ApiController]
    public class LancamentosController : ControllerBase
    {
        private readonly ILogger<LancamentosController> logger;

        private readonly IGeracaoDeProtocolos geracaoDeProtocolos;

        private readonly IMediator mediator;

        public LancamentosController(ILogger<LancamentosController> logger, IGeracaoDeProtocolos geracaoDeProtocolos, IMediator mediator)
        {
            this.logger = logger;

            this.geracaoDeProtocolos = geracaoDeProtocolos;

            this.mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ComandoDeLancamentoFinanceiro comando)
        {
            var protocolo = await geracaoDeProtocolos.GeraProtocolo();

            logger.LogDebug($"Protocolo: {protocolo}");

            comando.AnexaProtocolo(protocolo);

            //

            await mediator.Send(comando);

            //

            return Ok(protocolo);
        }
    }
}
