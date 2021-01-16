using FluxoDeCaixa.Modulos;
using MediatR;
using MediatR.Pipeline;
using MediatR.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Transactions;

namespace FluxoDeCaixa
{
    public static class InfraConfiguration
    {
        public static void AddMediatR<TService>(this IServiceCollection services) where TService : class
        {
            var serviceConfig = new MediatRServiceConfiguration();

            ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            var multiOpenInterfaces = new[]
            {
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IRequestPostProcessor<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>)
            };

            foreach (var multiOpenInterface in multiOpenInterfaces)
            {
                services.AddTransient(multiOpenInterface, typeof(TService));
            }
        }

        public static void AddInfraLancamentosApi(this IServiceCollection services)
        {
            services.AddModuloLancamentosParaProtocolo();
        }

        public static void AddInfraLancamentosWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUnitOfWork();

            services.AddDbContext(configuration);

            //var serviceConfig = new MediatRServiceConfiguration();

            //ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            //

            services.AddModuloLancamentos();
        }

        public static void AddInfraLancamentosTests(this IServiceCollection services)
        {
            services.AddUnitOfWork();

            services.AddDbContextInMemory();

            //var serviceConfig = new MediatRServiceConfiguration();

            //ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            //

            //services.AddModuloLancamentosTests();

            services.AddModuloLancamentos();
        }

        public static void AddInfraConsolidacaoWorkers(this IServiceCollection services)
        {
            //var serviceConfig = new MediatRServiceConfiguration();

            //ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            services.AddModuloConsolidacao();
        }

        public static void AddInfraConsultasApi(this IServiceCollection services)
        {
            //var serviceConfig = new MediatRServiceConfiguration();

            //ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            //

            services.AddModuloLancamentosParaConsultas();

            services.AddModuloConsolidacaoParaConsultas();

            services.AddModuloConsultas();
        }

        public static void AddInfraPoliticasWorkers(this IServiceCollection services, IConfiguration configuration)
        {
            //var serviceConfig = new MediatRServiceConfiguration();

            //ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            //

            services.AddModuloPoliticas();
        }

        private static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, TransactionScopeManager>();
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
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

        private static void AddDbContextInMemory(this IServiceCollection services)
        {
            var connectionString = @"DataSource=myshared.db";

            //var keepAliveConnection = new SqliteConnection(connectionString);

            //keepAliveConnection.Open();

            //var serviceProvider = new ServiceCollection()
            //    .AddEntityFrameworkSqlite()
            //    .BuildServiceProvider();

            services.AddDbContext<FluxoDeCaixaDbContext>(options =>
            {
                options.UseSqlite(connectionString);
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
