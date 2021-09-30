using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos.Politicas
{
    public static class PoliticasModule
    {
        internal static void AddPoliticaDeAplicacaoDeJuros(this IServiceCollection services)
        {
            services.AddPoliticas();
        }

        private static void AddPoliticas(this IServiceCollection services)
        {

        }
    }
}
