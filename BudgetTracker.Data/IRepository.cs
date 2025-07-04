using System.Linq.Expressions;

namespace BudgetTracker.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<T?> GetByIdAsync(int id, params string[] includes);
        Task<IEnumerable<T>> GetAllAsync(params string[] includes);

        Task<IEnumerable<T>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>>? orderBy = null,
            bool descending = false);

        Task<int> GetCountAsync();

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();
    }
}