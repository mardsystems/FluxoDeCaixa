using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private readonly IConsultaDeContas consultaDeContas;

        public ContasController(IConsultaDeContas consultaDeContas)
        {
            this.consultaDeContas = consultaDeContas;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var contas = await consultaDeContas.ConsultaContas();

            return Ok(contas);
        }
    }
}
