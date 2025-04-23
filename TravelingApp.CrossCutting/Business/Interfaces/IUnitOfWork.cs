namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IUnitOfWork<TContext>
        where TContext : class
    {
        bool HasScope { get; }
        TContext Context { get; }
        void AddPreCommitDelegate(Func<Task<bool>> action);
        IUnitOfWorkScope<TContext> CreateScope();
        Task CommitAsync(IUnitOfWorkScope<TContext> scope);
        void DisposeScope(IUnitOfWorkScope<TContext> scope);
    }
}
