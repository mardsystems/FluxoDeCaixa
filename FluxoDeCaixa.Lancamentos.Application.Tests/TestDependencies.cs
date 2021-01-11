using FluxoDeCaixa.Modulos;
using FluxoDeCaixa.Modulos.Lancamentos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;

namespace FluxoDeCaixa
{
    public static class TestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();

            services.AddUnitOfWork();

            services.AddLancamentos();

            services.AddDbContextInMemory();

            services.AddMediatR(typeof(ProcessadorDeLancamentosFinanceiros));

            return services;
        }
    }
}
