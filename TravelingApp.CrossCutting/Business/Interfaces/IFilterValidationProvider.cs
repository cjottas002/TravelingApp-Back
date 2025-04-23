using System.ComponentModel.DataAnnotations;

namespace TravelingApp.CrossCutting.Business.Interfaces
{
    public interface IFilterValidationProvider
    {
        void AddFilterValidation(string name, object filterTypes, Type type);

        Task ValidateFiltersAsync(IList<ValidationResult> validationResults, IEnumerable<IListFilter> listFilters);

        IDictionary<string, IFilterValidation> FilterValidations { get; }
    }
}
