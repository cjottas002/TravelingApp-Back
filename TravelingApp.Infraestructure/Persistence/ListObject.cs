using TravelingApp.CrossCutting.Business.Interfaces;

namespace TravelingApp.Infraestructure.Persistence
{
    public abstract class ListObject<T>() :  IListObject<T>
    {
        public IEnumerable<T>? Results { get; protected set; }

        protected abstract IQueryable<T> CreateQuery();
    }
}
