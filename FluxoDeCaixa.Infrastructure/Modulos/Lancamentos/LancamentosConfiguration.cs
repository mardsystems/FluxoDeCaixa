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

        public static void AddPagamentos(this IServiceCollection services)
        {

        }

        public static void AddLancamentos(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeLancamentos, LancamentosDbService>();

            services.AddMediatR(
                typeof(ProcessamentoDeLancamentosFinanceirosRabbitMQService),
                typeof(ProcessadorDeLancamentosFinanceiros),
                typeof(LancamentosFinanceirosProcessadosRabbitMQService)
            );
        }

        public static void AddLancamentosParaProtocolamento(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ProtocolamentoDeLancamentosFinanceirosRabbitMQService));
        }

        public static void AddLancamentosParaConsolidacao(this IServiceCollection services)
        {

        }

        public static void AddContas(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeContas, ContasDbService>();
        }

        public static void AddContasParaConsultas(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ContasMongoDBService));
        }
    }
}
