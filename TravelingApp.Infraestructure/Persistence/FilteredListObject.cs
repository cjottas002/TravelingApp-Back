using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TravelingApp.Application.Request;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Extensions;

namespace TravelingApp.Infraestructure.Persistence
{
    public abstract class FilteredListObject<T>(IFilterValidationProvider filterValidationProvider) : ListObject<T>, IFilteredListObject<T>
        where T : class
    {
        protected FrameworkRequest? Page { get; set; }

        private readonly IFilterValidationProvider filterValidationProvider = filterValidationProvider;

        public ICollection<IListFilter> Filters { get; protected set; } = [];

        protected IDictionary<string, IFilterValidation> ValidFilters { get; } = new Dictionary<string, IFilterValidation>();

        public IList<ValidationResult> ValidationResults { get; private set; } = [];

        protected bool IsValid {  get; set; } = true;


        public int TotalResults { get; set; }

        protected async Task<bool> ValidationAsync(IList<ValidationResult> validationResults)
        {
            if (this.Page?.PageIndex < 1)
            {
                validationResults.Add(new ValidationResult("PageIndex debe ser mayor o igual a 1", [nameof(this.Page.PageIndex)]));
                this.IsValid = false;
            }

            if (this.Page?.PageSize <= 0 || this.Page?.PageSize > 500)
            {
                validationResults.Add(new ValidationResult("PageSize debe estar entre 1 y 500", [nameof(this.Page.PageSize)]));
                this.IsValid = false;
            }

            await ValidateFiltersAsync(validationResults);
            return this.IsValid;
        }

        public async Task<bool> ExecutePagedAsync(int pageSize, int pageIndex, string orderBy, bool ascending)
        {
            var isValidated = await this.ValidationAsync(this.ValidationResults);
            if (!isValidated) return false;

            IQueryable<T> query = this.CreateQuery() ?? throw new Exception("IQueryable<T> not returned from CreateQuery()");
            this.TotalResults = await query.CountAsync();
            this.Results = await query.Page(pageSize, pageIndex, orderBy, ascending).ToListAsync();
            return true;
        }

        protected void AddFilterValidation(string name, object filterTypes, Type type)
        {
            this.filterValidationProvider.AddFilterValidation(name, filterTypes, type);
        }

        protected async Task ValidateFiltersAsync(IList<ValidationResult> validationResults)
        {
            if(this.Filters.Count == 0) return;
            await this.filterValidationProvider.ValidateFiltersAsync(validationResults, this.Filters);
        }

        public void InitializeValidationResults()
        {
            ValidationResults ??= [];
        }

        public bool Validate(IList<ValidationResult>? validationResults = null)
        {
            InitializeValidationResults();
            bool result = Validator.TryValidateObject(this, new ValidationContext(this, null, null), ValidationResults, true);

            if (!result && validationResults != null)
            {
                foreach (var item in ValidationResults)
                {
                    validationResults.Add(item);
                }
            }

            return result;
        }
    }
}
