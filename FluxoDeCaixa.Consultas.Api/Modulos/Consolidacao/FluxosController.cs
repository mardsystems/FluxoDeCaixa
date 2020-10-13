using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    [Route("api/consultas/[controller]")]
    [ApiController]
    public class FluxosController : ControllerBase
    {
        private readonly IMediator mediator;

        public FluxosController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{dia:int},{mes:int},{ano:int}")]
        public async Task<IActionResult> Get(int dia, int mes, int ano)
        {
            var request = new SolicitacaoDeConsultaDeFluxoDeCaixa
            {
                Dia = dia,
                Mes = mes,
                Ano = ano
            };

            var fluxo = await mediator.Send(request);

            return Ok(fluxo);
        }
    }
}
