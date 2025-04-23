namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IListObject<T>
    {
        IEnumerable<T>? Results { get; }
    }
}
