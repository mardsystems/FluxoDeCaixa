using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Fluxos
{
    public static class ConsolidacaoConfiguration
    {
        public static void AddConsolidacaoDeLancamentos(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeFluxoDeCaixa, ConsolidacaoMongoDBService>();

            services.AddMediatRTypes(
                typeof(ConsolidadorDeLancamentosProcessados),
                typeof(ConsolidacaoDeLancamentosProcessadosRabbitMQService)
            );
        }

        public static void AddConsolidacaoParaConsultas(this IServiceCollection services)
        {
            services.AddMediatRTypes(typeof(ConsolidacaoMongoDBService));
        }
    }
}
