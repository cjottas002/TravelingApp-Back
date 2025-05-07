using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using TravelingApp.Application.Common.Interfaces;
using TravelingApp.Application.Request;
using TravelingApp.CrossCutting.Business.Interfaces;
using TravelingApp.CrossCutting.Configuration;
using TravelingApp.CrossCutting.Extensions;

namespace TravelingApp.Infraestructure.Persistence
{
    public abstract class FilteredListObject<T>(
        IFilterValidationProvider filterValidationProvider, 
        ICacheService cacheService,
        IOptions<RedisOptions> redisConfigOptions
        ) : ListObject<T>, IFilteredListObject<T>
        where T : class
    {
        private readonly ICacheService cacheService = cacheService.ValidateArgument();
        private readonly RedisOptions redifConfiguration = redisConfigOptions.ValidateArgument().Value;
        private readonly IFilterValidationProvider filterValidationProvider = filterValidationProvider.ValidateArgument();

        public ICollection<IListFilter> Filters { get; protected set; } = [];
        public IList<ValidationResult> ValidationResults { get; private set; } = [];
        public int TotalResults { get; set; }

        protected FrameworkRequest? Page { get; set; }
        protected IDictionary<string, IFilterValidation> ValidFilters { get; } = new Dictionary<string, IFilterValidation>();
        protected bool IsValid {  get; set; } = true;

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

            string cacheKey = $"Core:{typeof(T).Name}:page={pageIndex}:size={pageSize}:sort={orderBy}:{(ascending ? "asc" : "desc")}";

            var cached = await cacheService.GetAsync<List<T>>(cacheKey);
            if (cached is not null)
            {
                this.Results = cached;
                this.TotalResults = cached.Count;
                //Logger.LogTrace("Resultados obtenidos de caché con clave {CacheKey}", cacheKey);
                return true;
            }


            IQueryable<T>? query = CreateQuery();
            if (query == null)
            {
                //Logger.LogError("CreateQuery() devolvió null. No se puede ejecutar la consulta.");
                return false;
            }

            this.TotalResults = await query.CountAsync();
            this.Results = await query.Page(pageSize, pageIndex, orderBy, ascending).ToListAsync();

            await cacheService.SetAsync(cacheKey, this.Results, redifConfiguration.SlidingExpiration, redifConfiguration.AbsoluteExpirationRelativeToNow);
            //Logger.LogTrace("Resultados almacenados en caché con clave {CacheKey}", cacheKey);

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
