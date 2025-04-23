using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using EFCore.BulkExtensions;

namespace TravelingApp.CrossCutting.Business.Interfaces
{
    /// <summary>
    /// Repositorio genérico para operaciones CRUD, consultas avanzadas y bulk insert.
    /// </summary>
    public interface IRepository<T, E>
        where T : class
        where E : DbContext
    {
        // Agregar entidades
        void Add(T entity);
        void Add(IEnumerable<T> entities);

        // Eliminar entidades
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);

        // Editar entidades
        void Edit(T entity);

        // Insert masivo (con y sin configuración personalizada)
        void BulkInsert(IEnumerable<T> entities);
        Task BulkInsertAsync(IEnumerable<T> entities);
        void BulkInsert(IEnumerable<T> entities, BulkConfig options);
        Task BulkInsertAsync(IEnumerable<T> entities, BulkConfig options);

        // Obtener colección base
        IQueryable<T> Entity(bool asNoTracking = false);

        // Consultas con predicados
        IQueryable<T> Entity(Expression<Func<T, bool>> where);
        IQueryable<T> Find(Expression<Func<T, bool>> where);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] navigationProperties);

        // Acceso a la vista local (solo en tracking)
        LocalView<T> EntityLocal();

        // Comprobación de existencia
        bool Exists(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Obtener único elemento o excepción
        T Single(Expression<Func<T, bool>> where);
        Task<T> SingleAsync(Expression<Func<T, bool>> where);
        T SingleOrDefault(Expression<Func<T, bool>> where);
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> where);

        // Obtener primero o excepción
        T First(Expression<Func<T, bool>> where);
        Task<T> FirstAsync(Expression<Func<T, bool>> where);
        T FirstOrDefault(Expression<Func<T, bool>> where);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where);

        // Adjuntar y desacoplar del contexto
        void Attach(T entity);
        void Detach(T entity);

        // Rango de actualizaciones/eliminaciones
        void UpdateRange(IEnumerable<T> entities);
        void RemoveRange(IEnumerable<T> entities);
    }
}
