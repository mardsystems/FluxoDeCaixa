using FluxoDeCaixa.Modulos.Fluxos;
using FluxoDeCaixa.Modulos.Lancamentos;
using FluxoDeCaixa.Modulos.Politicas;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa
{
    public static class InfraModule
    {
        public static void AddInfraLancamentosApi(this IServiceCollection services)
        {
            services.AddMediatRCore();

            services.AddProtocolamentoDeLancamentos();

            services.AddMediatR();
        }

        public static void AddInfraLancamentosWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUnitOfWork();

            services.AddDbContext(configuration);

            services.AddMediatRCore();

            services.AddProcessamentoDeLancamentos();

            services.AddMediatR();
        }

        public static void AddInfraApplicationTests(this IServiceCollection services)
        {
            services.AddUnitOfWork();

            services.AddDbContextInMemory();

            services.AddMediatRCore();

            services.AddProcessamentoDeLancamentos();

            services.AddMediatR();
        }

        public static void AddInfraFluxosWorkers(this IServiceCollection services)
        {
            services.AddMediatRCore();

            services.AddConsolidacaoDeLancamentos();

            services.AddMediatR();
        }

        public static void AddInfraFluxosApi(this IServiceCollection services)
        {
            services.AddMediatRCore();

            services.AddConsultaDeFluxoDeCaixaDoDia();

            services.AddConsultaDeContas();

            services.AddMediatR();
        }

        public static void AddInfraPoliticasWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatRCore();

            services.AddPoliticaDeAplicacaoDeJuros();

            services.AddMediatR();
        }
    }
}
