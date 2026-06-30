using System.Linq.Expressions;

namespace Application.Services.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetPagedResultAsync(Expression<Func<T, bool>>? predicate, int skip, int take);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate);
    }
}
