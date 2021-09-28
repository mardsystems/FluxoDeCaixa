using Microsoft.Extensions.Hosting;

namespace FluxoDeCaixa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddInfraPoliticasWorkers(hostContext.Configuration);
                });
    }
}
