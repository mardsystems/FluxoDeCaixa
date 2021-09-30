using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Fluxos
{
    public static class FluxosModule
    {
        internal static void AddConsolidacaoDeLancamentos(this IServiceCollection services)
        {
            services.AddMediatRTypes(
                typeof(ConsolidacaoDeLancamentosProcessadosRabbitMQService),
                typeof(ConsolidadorDeLancamentosProcessados)
            );

            services.AddFluxoDeCaixa();
            
            //services.AddLancamentosParaConsolidacao();
        }

        internal static void AddConsultaDeFluxoDeCaixaDoDia(this IServiceCollection services)
        {
            services.AddMediatRTypes(typeof(ConsolidacaoMongoDBService));
        }

        private static void AddFluxoDeCaixa(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeFluxoDeCaixa, ConsolidacaoMongoDBService>();
        }
    }
}
