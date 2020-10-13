using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class LancamentosConfiguration
    {
        public static void AddProtocolos(this IServiceCollection services)
        {
            services.AddTransient<IGeracaoDeProtocolos, ProtocolosMongoDBService>();
        }

        public static void AddLancamentos(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeLancamentos, LancamentosDbService>();

            services.AddTransient<IRepositorioDeContas, ContasDbService>();

            services.AddMediatR(typeof(ProcessadorDeLancamentosFinanceiros));
        }

        public static void AddLancamentosParaConsultas(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ContasMongoDBService));
        }
    }
}
