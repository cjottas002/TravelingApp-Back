using Microsoft.EntityFrameworkCore;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Extensions;
using TravelingApp.Infraestructure.Repositories;

namespace TravelingApp.Infraestructure.Persistence
{
    public class UnitOfWork<TContext>(TContext context) : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly List<Func<Task<bool>>> preCommitDelegates = [];

        internal IUnitOfWorkScope<TContext>? CurrentScope { get; set; }
        public bool HasScope => CurrentScope != null;


        public TContext Context { get; } = context.ValidateArgument();

        public IUnitOfWorkScope<TContext> CreateScope()
        {
            if (HasScope && this.CurrentScope is not null)
            {
                return this.CurrentScope;
            }

            return CurrentScope = new UnitOfWorkScope<TContext>(this);
        }

        public void AddPreCommitDelegate(Func<Task<bool>> action)
        {
            preCommitDelegates.Add(action);
        }

        public async Task CommitAsync(IUnitOfWorkScope<TContext> scope)
        {
            if (scope == null || scope != this.CurrentScope)
            {
                throw new InvalidOperationException("Invalid scope for commit.");
            }

            await RunPreCommitDelegatesAsync();
            await Context.SaveChangesAsync();
        }

        public void DisposeScope(IUnitOfWorkScope<TContext> scope)
        {
            ArgumentNullException.ThrowIfNull(scope);

            if (scope != CurrentScope)
            {
                CurrentScope = null;
            }

            preCommitDelegates.Clear();

        }


        private async Task RunPreCommitDelegatesAsync()
        {
            foreach (var action in preCommitDelegates)
            {
                var result = await action();
                if (!result)
                    throw new InvalidOperationException($"A pre-commit delegate failed: {action.Method.Name}");
            }

        }
    }
}
