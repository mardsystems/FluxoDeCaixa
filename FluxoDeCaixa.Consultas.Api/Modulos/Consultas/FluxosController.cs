using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consultas
{
    [Route("api/consultas/[controller]")]
    [ApiController]
    public class FluxosController : ControllerBase
    {
        private readonly IConsultaDeFluxoDeCaixa consultaDeFluxoDeCaixa;

        public FluxosController(IConsultaDeFluxoDeCaixa consultaDeFluxoDeCaixa)
        {
            this.consultaDeFluxoDeCaixa = consultaDeFluxoDeCaixa;
        }

        [HttpGet("{dia:int},{mes:int},{ano:int}")]
        public async Task<IActionResult> Get(int dia, int mes, int ano)
        {
            var fluxo = await consultaDeFluxoDeCaixa.ConsultaFluxoDeCaixa(dia, mes, ano);

            return Ok(fluxo);
        }
    }
}
