using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public static class ConsolidacaoConfiguration
    {
        public static void AddConsolidacao(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeFluxos, ConsolidacaoDbService>();

            services.AddMediatR(typeof(ServicoDeConsolidacaoDeFinancas));
        }
    }
}
