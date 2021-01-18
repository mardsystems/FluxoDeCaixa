using FluxoDeCaixa.Modulos.Consolidacao;
using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Modulos
{
    public static class ModulosConfiguration
    {
        #region Lancamentos

        public static void AddModuloLancamentos(this IServiceCollection services)
        {
            //services.AddProtocolos();

            services.AddPagamentos();

            services.AddLancamentos();

            services.AddContas();
        }

        public static void AddModuloLancamentosParaProtocolamento(this IServiceCollection services)
        {
            services.AddProtocolos();

            services.AddLancamentosParaProtocolamento();
        }

        public static void AddModuloLancamentosParaConsultas(this IServiceCollection services)
        {
            services.AddContasParaConsultas();
        }

        #endregion

        #region Consolidacao

        public static void AddModuloConsolidacao(this IServiceCollection services)
        {
            services.AddLancamentosParaConsolidacao();

            services.AddConsolidacaoDeLancamentos();
        }

        public static void AddModuloConsolidacaoParaConsultas(this IServiceCollection services)
        {
            services.AddConsolidacaoParaConsultas();
        }

        #endregion

        #region Consultas

        public static void AddModuloConsultas(this IServiceCollection services)
        {

        }

        #endregion

        #region Politicas

        public static void AddModuloPoliticas(this IServiceCollection services)
        {

        }

        #endregion
    }
}
