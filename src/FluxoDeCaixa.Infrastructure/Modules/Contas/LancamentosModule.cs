using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modules.Contas
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
                typeof(ProcessadorDeLancamentosProtocoladosRabbitMQ),
                typeof(ProcessadorDeLancamentos),
                typeof(PublicadorDeLancamentosProcessadosRabbitMQ)
            );

            services.AddContas();
        }

        private static void AddContas(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeContas, ContasDbService>();
        }

        internal static void AddProtocolamentoDeLancamentos(this IServiceCollection services)
        {
            services.AddMediatRTypes(typeof(ProtocoladorDeLancamentosRabbitMQ));
        }

        internal static void AddGeracaoDeProtocolos(this IServiceCollection services)
        {
            services.AddTransient<IGeracaoDeProtocolos, ProtocolosMongoDBService>();
        }
    }
}
