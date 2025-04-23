namespace TravelingApp.CrossCutting.Enums
{
    public enum FilterType
    {
        Equal = 0,
        Like = 1,
        GreaterThan = 2,
        GreaterThanOrEqual = 3,
        LessThan = 4,
        LessThanOrEqual = 5,
        NotEqual = 6,
        IsNull = 7,
        IsNotNull = 8,
        Any = 9,
        None = 10,
        And = 11,
        Or = 12,
        StartsWith = 13,
        EndsWith = 1
    }
}
