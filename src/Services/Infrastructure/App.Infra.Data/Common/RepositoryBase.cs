using App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace App.Infra.Data.Common;

public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>, IDisposable
    where TEntity : class, IAggregateRoot<TKey>, new()
{
    protected AppDbContext _context;
    protected DbConnection _connection;

    protected DbConnection DbConnection
    {
        get
        {
            _connection ??= _context.Database.GetDbConnection();
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            return _connection;
        }
    }

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context?.Dispose();
            _context = null;
            _connection?.Dispose();
            _connection = null;
        }
    }

    public Task<TEntity> FindByIdAsync(TKey id)
    {
        return Filter(x => x.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task<T> AddAsync<T>(T entity) where T : class, IEntity
    {
        await _context.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.AddRangeAsync(entities);
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _context.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }

    public void Update<T>(T entity) where T : class, IEntity
    {
        _context.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _context.UpdateRange(entities);
    }

    public void Remove(TEntity entity)
    {
        _context.Remove(entity);
    }

    public void Remove<T>(T entity) where T : class, IEntity
    {
        _context.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            _context.Remove((object)entity);
        }
    }

    public void RemoveRange<T>(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Remove(entity);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    protected IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> expression)
    {
        return _context.Set<TEntity>().Where(expression);
    }

    protected IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression) where T : class
    {
        return _context.Set<T>().Where(expression);
    }
}
