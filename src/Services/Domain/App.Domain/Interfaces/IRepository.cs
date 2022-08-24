using System.Linq.Expressions;

namespace App.Domain.Interfaces;

public interface IRepository<TEntity, TKey> where TEntity : IAggregateRoot<TKey>
{
    Task<TEntity> FindByIdAsync(TKey id);
    Task<TEntity> Add(TEntity entity);
    Task<T> AddAsync<T>(T entity) where T : class;
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
    void Update(TEntity entity);
    void Update<T>(T entity) where T : class;
    void Remove(TEntity entity);
    void Remove<T>(T entity) where T : class;
    void RemoveRange(IEnumerable<TEntity> entities);
    void RemoveRange<T>(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();
}
