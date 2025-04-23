namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IFilterValidation
    {
        object FilterTypes { get; set; }

        Type TargetType { get; set; }
    }
}
