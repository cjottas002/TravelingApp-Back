using TravelingApp.CrossCutting.Enums;

namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IListFilter
    {
        public string? By { get; set; }

        public FilterType Type { get; set; }

        public object? Value { get; set; }
    }
}
