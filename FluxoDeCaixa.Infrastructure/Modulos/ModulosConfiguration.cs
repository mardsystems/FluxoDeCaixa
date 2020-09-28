using FluxoDeCaixa.Modulos.Consolidacao;
using FluxoDeCaixa.Modulos.Consultas;
using FluxoDeCaixa.Modulos.Lancamentos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluxoDeCaixa.Modulos
{
    public static class ModulosConfiguration
    {
        public static void AddModulos(this IServiceCollection services)
        {
            var connectionString = "DataSource=c:\\temp\\myshared.db";

            //var keepAliveConnection = new SqliteConnection(connectionString);

            //keepAliveConnection.Open();

            //var serviceProvider = new ServiceCollection()
            //    .AddEntityFrameworkSqlite()
            //    .BuildServiceProvider();

            services.AddDbContext<FluxoDeCaixaDbContext>(options =>
            {
                options.UseSqlite(connectionString);

                //options.UseInternalServiceProvider(serviceProvider);
            });

            var contextServiceProvider = services.BuildServiceProvider();

            using (var scope = contextServiceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;

                //var logger = scopedProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                using (var context = scopedProvider.GetRequiredService<FluxoDeCaixaDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }

            services.AddLancamentos();

            services.AddConsolidacao();

            services.AddConsultas();
        }
    }
}
