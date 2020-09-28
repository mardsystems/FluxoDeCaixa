using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Lancamentos
{
    public static class LancamentosConfiguration
    {
        public static void AddLancamentos(this IServiceCollection services)
        {
            services.AddTransient<IRepositorioDeLancamentos, LancamentosDbService>();

            services.AddTransient<IConsultaDeContas, ContasDbService>();

            services.AddTransient<IRepositorioDeContas, ContasDbService>();

            services.AddMediatR(typeof(ProcessadorDeLancamentosFinanceiros));
        }
    }
}
