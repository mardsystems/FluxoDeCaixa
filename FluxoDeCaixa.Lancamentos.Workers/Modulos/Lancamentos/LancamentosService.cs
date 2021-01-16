using MediatR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public class LancamentosService : BackgroundService
    {
        private readonly IMediator mediator;

        public LancamentosService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await mediator.Send(new ComandoParaIniciarLancamentoFinanceiro());
        }
    }
}
