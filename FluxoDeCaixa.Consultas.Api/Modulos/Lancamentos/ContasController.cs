using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Route("api/lancamentos/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly IMediator mediator;

        public ContasController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var request = new SolicitacaoDeConsultaDeContas();

            var contas = await mediator.Send(request);

            return Ok(contas);
        }
    }
}
