using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Consolidacao
{
    public static class ConsolidacaoConfiguration
    {
        public static void AddConsolidacao(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeFluxos, ConsolidacaoMongoDBService>();

            services.AddMediatR(typeof(ServicoDeConsolidacaoDeFinancas));
        }

        public static void AddConsolidacaoParaConsultas(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ConsolidacaoMongoDBService));
        }
    }
}
