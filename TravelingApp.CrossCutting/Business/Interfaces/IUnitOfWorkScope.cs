namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IUnitOfWorkScope<T> : IDisposable
    {
        Task CommitAsync();
    }
}
