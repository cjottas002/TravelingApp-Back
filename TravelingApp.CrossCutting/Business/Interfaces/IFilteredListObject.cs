namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IFilteredListObject<T> : IListObject<T>
    {
        ICollection<IListFilter> Filters { get; }
    }
}
