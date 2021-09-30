using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace FluxoDeCaixa
{
    public static class SystemModule
    {
        internal static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, TransactionScopeManager>();
        }
    }
}
