namespace TravelingApp.Infraestructure.Repositories
{
    using EFCore.BulkExtensions;
    using global::TravelingApp.CrossCutting.Business.Interfaces;
    using global::TravelingApp.CrossCutting.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Linq.Expressions;

    namespace TravelingApp.Infrastructure.Repositories
    {
        public class Repository<TEntity, TContext>(IUnitOfWork<TContext> unitOfWork) : IRepository<TEntity, TContext>
            where TEntity : class, new()
            where TContext : DbContext
        {
            internal IUnitOfWork<TContext> UnitOfWork { get; private set; } = unitOfWork.ValidateArgument();
            internal DbSet<TEntity> Entities { get; set; } = unitOfWork.Context.Set<TEntity>();

            internal TContext DbContext => UnitOfWork.Context as TContext;

            public string EntityName => DbContext.Model.FindEntityType(typeof(TEntity))?.GetTableName() ?? string.Empty;

            public void Add(TEntity entity) => Entities.Add(entity);

            public void Add(IEnumerable<TEntity> entities) => Entities.AddRange(entities);

            public void Delete(TEntity entity) => Entities.Remove(entity);

            public void Delete(IEnumerable<TEntity> entities) => Entities.RemoveRange(entities);

            public void Edit(TEntity entity)
            {
                Entities.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }

            public void Attach(TEntity entity) => DbContext.Attach(entity);

            public void Detach(TEntity entity) => DbContext.Entry(entity).State = EntityState.Detached;

            public void UpdateRange(IEnumerable<TEntity> entities) => Entities.UpdateRange(entities);

            public void RemoveRange(IEnumerable<TEntity> entities) => Entities.RemoveRange(entities);

            public void BulkInsert(IEnumerable<TEntity> entities)
            {
                var options = new BulkConfig { EnableStreaming = true, UseTempDB = true };
                DbContext.BulkInsert(entities.ToList(), options);
            }

            public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
            {
                var options = new BulkConfig { EnableStreaming = true, UseTempDB = true };
                await DbContext.BulkInsertAsync(entities.ToList(), options);
            }

            public void BulkInsert(IEnumerable<TEntity> entities, BulkConfig options)
            {
                options ??= new BulkConfig { EnableStreaming = true, UseTempDB = true };
                DbContext.BulkInsert(entities.ToList(), options);
            }

            public async Task BulkInsertAsync(IEnumerable<TEntity> entities, BulkConfig options)
            {
                options ??= new BulkConfig { EnableStreaming = true, UseTempDB = true };
                await DbContext.BulkInsertAsync(entities.ToList(), options);
            }

            public IQueryable<TEntity> Entity(bool asNoTracking = false) =>
                asNoTracking ? Entities.AsNoTracking().AsQueryable() : Entities;

            public IQueryable<TEntity> Entity(Expression<Func<TEntity, bool>> where) => Entities.Where(where);

            public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] navigationProperties)
            {
                IQueryable<TEntity> query = Entity();
                foreach (var include in navigationProperties)
                    query = query.Include(include);
                return query;
            }

            public LocalView<TEntity> EntityLocal() => Entities.Local;

            public bool Exists(Expression<Func<TEntity, bool>> predicate) => Entities.Any(predicate);

            public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
                await Entities.AnyAsync(predicate);

            public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> where) => Entities.Where(where);

            public TEntity Single(Expression<Func<TEntity, bool>> where) => Entities.Single(where);

            public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where) =>
                await Entities.SingleAsync(where);

            public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where) => Entities.SingleOrDefault(where);

            public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where) =>
                await Entities.SingleOrDefaultAsync(where);

            public TEntity First(Expression<Func<TEntity, bool>> where) => Entities.First(where);

            public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where) =>
                await Entities.FirstAsync(where);

            public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where) => Entities.FirstOrDefault(where);

            public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where) =>
                await Entities.FirstOrDefaultAsync(where);
        }
    }
}
