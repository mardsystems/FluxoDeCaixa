using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace FluxoDeCaixa
{
    public static class DatabaseModule
    {
        internal static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
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
                options.UseLazyLoadingProxies();

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
                    //Thread.Sleep(25000);

                    //context.Database.EnsureDeleted();

                    context.Database.EnsureCreated();
                }
            }
        }

        internal static void AddDbContextInMemory(this IServiceCollection services)
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
                    //Thread.Sleep(25000);

                    context.Database.EnsureDeleted();

                    context.Database.EnsureCreated();
                }
            }
        }

        public static DataTable ExecuteForTest(string sql)
        {
            using (var connection = new SqliteConnection("DataSource=myshared.db"))
            {
                connection.Open();

                try
                {
                    using (var command = new SqliteCommand(sql, connection))
                    {
                        var reader = command.ExecuteReader();

                        var table = new DataTable();

                        table.Load(reader);

                        return table;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
