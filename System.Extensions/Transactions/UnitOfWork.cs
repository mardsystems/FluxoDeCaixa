using System.Threading.Tasks;

namespace System.Transactions
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();

        Task Commit();

        Task Rollback();
    }

    public class TransactionScopeManager : IUnitOfWork
    {
        private TransactionScope scope;

        public async Task BeginTransaction()
        {
            scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await Task.CompletedTask;
        }

        public async Task Commit()
        {
            scope.Complete();

            scope.Dispose();

            scope = null;

            await Task.CompletedTask;
        }

        public async Task Rollback()
        {
            scope.Dispose();

            scope = null;

            await Task.CompletedTask;
        }
    }

    public class DummyUnitOfWork : IUnitOfWork
    {
        public async Task BeginTransaction()
        {
            await Task.Run(() => { });
        }

        public async Task Commit()
        {
            await Task.Run(() => { });
        }

        public async Task Rollback()
        {
            await Task.Run(() => { });
        }
    }
}
