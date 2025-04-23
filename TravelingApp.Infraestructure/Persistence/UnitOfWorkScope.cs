using Microsoft.EntityFrameworkCore;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Extensions;

namespace TravelingApp.Infraestructure.Repositories
{
    public class UnitOfWorkScope<TContext>(IUnitOfWork<TContext> unitOfWork) : IUnitOfWorkScope<TContext>
        where TContext : DbContext
    {
        private readonly IUnitOfWork<TContext> unitOfWork = unitOfWork.ValidateArgument();

        public async Task CommitAsync()
        {
            await this.unitOfWork.CommitAsync(this);
        }

        public void Dispose()
        {
            this.unitOfWork.DisposeScope(this);
            GC.SuppressFinalize(this);
        }
    }
}
