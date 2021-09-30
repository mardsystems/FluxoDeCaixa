using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class LancamentosModule
    {
        internal static void AddConsultaDeContas(this IServiceCollection services)
        {
            services.AddMediatRTypes(typeof(ContasMongoDBService));
        }

        internal static void AddProcessamentoDeLancamentos(this IServiceCollection services)
        {
            services.AddMediatRTypes(
                typeof(ProcessamentoDeLancamentosFinanceirosRabbitMQService),
                typeof(ProcessadorDeLancamentosFinanceiros),
                typeof(LancamentosFinanceirosProcessadosRabbitMQService)
            );

            services.AddContas();
        }

        private static void AddContas(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeContas, ContasDbService>();
        }

        internal static void AddProtocolamentoDeLancamentos(this IServiceCollection services)
        {
            services.AddMediatRTypes(typeof(ProtocolamentoDeLancamentosFinanceirosRabbitMQService));

            services.AddProtocolos();
        }

        private static void AddProtocolos(this IServiceCollection services)
        {
            services.AddTransient<IGeracaoDeProtocolos, ProtocolosMongoDBService>();
        }
    }
}
