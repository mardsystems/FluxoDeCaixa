using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Consultas
{
    public static class ConsultasConfiguration
    {
        public static void AddConsultas(this IServiceCollection services)
        {
            services.AddTransient<IConsultaDeFluxoDeCaixa, ConsultasDbService>();
        }
    }
}
