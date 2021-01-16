using MediatR;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public class ConsolidacaoService : BackgroundService
    {
        private readonly IMediator mediator;

        public ConsolidacaoService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await mediator.Send(new ComandoParaIniciarConsolidacao());
        }
    }
}
