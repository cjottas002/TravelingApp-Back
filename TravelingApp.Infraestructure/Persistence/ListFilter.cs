using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Enums;

namespace TravelingApp.Infraestructure.Persistence
{
    public class ListFilter : IListFilter
    {
        public string? By { get; set; }
        public FilterType Type { get; set; }
        public object? Value { get; set; }
    }
}
