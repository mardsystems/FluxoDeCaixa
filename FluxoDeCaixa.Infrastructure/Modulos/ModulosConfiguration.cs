using FluxoDeCaixa.Modulos.Consolidacao;
using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Transactions;

namespace FluxoDeCaixa.Modulos
{
    public static class ModulosConfiguration
    {
        public static void AddModulosParaProtocolos(this IServiceCollection services)
        {
            services.AddProtocolos();
        }

        public static void AddModulosParaLancamentos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUnitOfWork();

            services.AddDbContext(configuration);

            //

            services.AddLancamentos();
        }

        public static void AddModulosParaConsolidacao(this IServiceCollection services)
        {
            services.AddConsolidacao();
        }

        public static void AddModulosParaConsultas(this IServiceCollection services)
        {
            services.AddLancamentosParaConsultas();

            services.AddConsolidacaoParaConsultas();
        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, TransactionScopeManager>();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnection = configuration.GetConnectionString("SqlConnection");

            //var connectionString = @"DataSource=myshared.db";

            //var keepAliveConnection = new SqliteConnection(connectionString);

            //keepAliveConnection.Open();

            //var serviceProvider = new ServiceCollection()
            //    .AddEntityFrameworkSqlite()
            //    .BuildServiceProvider();

            services.AddDbContext<FluxoDeCaixaDbContext>(options =>
            {
                options.UseSqlServer(sqlConnection);

                //options.UseSqlite(connectionString);

                //options.UseInternalServiceProvider(serviceProvider);
            });

            var contextServiceProvider = services.BuildServiceProvider();

            using (var scope = contextServiceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                //var logger = scopedProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                using (var context = scopedProvider.GetRequiredService<FluxoDeCaixaDbContext>())
                {
                    Thread.Sleep(25000);

                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
