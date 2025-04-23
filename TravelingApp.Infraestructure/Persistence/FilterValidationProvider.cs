using System.ComponentModel.DataAnnotations;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Enums;

namespace TravelingApp.Infraestructure.Persistence
{
    public class FilterValidationProvider : IFilterValidationProvider
    {
        private readonly Dictionary<string, IFilterValidation> _filterValidations;

        public FilterValidationProvider()
        {
            _filterValidations = [];
        }

        public IDictionary<string, IFilterValidation> FilterValidations
        {
            get { return _filterValidations; }
        }

        public void AddFilterValidation(string name, object filterTypes, Type type)
        {
            if (!typeof(IFilterValidation).IsAssignableFrom(type))
            {
                throw new ArgumentException($"El tipo proporcionado {type.FullName} debe implementar IFilterValidation.");
            }

            var instance = Activator.CreateInstance(type) as IFilterValidation 
                ?? throw new InvalidOperationException($"No se pudo crear una instancia del tipo {type.FullName}. Asegúrate de que tenga un constructor público sin parámetros.");
            instance.FilterTypes = filterTypes;
            instance.TargetType = type;

            if (_filterValidations.ContainsKey(name))
            {
                throw new InvalidOperationException($"Ya existe una validación de filtro con el nombre '{name}'.");
            }

            _filterValidations.Add(name, instance);
        }


        public Task ValidateFiltersAsync(IList<ValidationResult> validationResults, IEnumerable<IListFilter> listFilters)
        {
            ArgumentNullException.ThrowIfNull(validationResults);
            ArgumentNullException.ThrowIfNull(listFilters);

            foreach (var filter in listFilters)
            {
                if (string.IsNullOrWhiteSpace(filter.By))
                {
                    validationResults.Add(new ValidationResult("El nombre del campo (By) no puede ser nulo o vacío."));
                    continue;
                }

                if (!_filterValidations.TryGetValue(filter.By, out var filterValidation))
                {
                    validationResults.Add(new ValidationResult($"No se encontró una validación para el filtro '{filter.By}'."));
                    continue;
                }

                var allowedTypes = (IEnumerable<FilterType>)filterValidation.FilterTypes;
                if (!allowedTypes.Contains(filter.Type))
                {
                    validationResults.Add(new ValidationResult(
                        $"El tipo de filtro '{filter.Type}' no está permitido para el campo '{filter.By}'."));
                    continue;
                }

                if (filter.Value != null)
                {
                    var expectedType = filterValidation.TargetType;
                    var valueType = filter.Value.GetType();

                    if (valueType != expectedType &&
                        !(filter.Value is string && expectedType.IsEnum))
                    {
                        validationResults.Add(new ValidationResult(
                            $"El valor para '{filter.By}' debe ser del tipo '{expectedType.Name}', pero fue '{valueType.Name}'."));
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
