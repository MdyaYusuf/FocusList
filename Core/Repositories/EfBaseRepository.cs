
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class EfBaseRepository<TContext, TEntity, TId> : IRepository<TEntity, TId>
  where TEntity : Entity<TId>, new()
  where TContext : DbContext
{
  protected TContext _context { get; }
  public EfBaseRepository(TContext context)
  {
    _context = context;
  }

  public async Task<TEntity> AddAsync(TEntity entity)
  {
    entity.CreatedDate = DateTime.Now;
    await _context.Set<TEntity>().AddAsync(entity);
    await _context.SaveChangesAsync();
    return entity;
  }

  public async Task<List<TEntity>> GetAllAsync()
  {
    return await _context.Set<TEntity>().ToListAsync();
  }

  public async Task<TEntity?> GetByIdAsync(TId id)
  {
    return await _context.Set<TEntity>().FindAsync(id);
  }

  public async Task<TEntity?> RemoveAsync(TEntity entity)
  {
    _context.Set<TEntity>().Remove(entity);
    await _context.SaveChangesAsync();
    return entity;
  }

  public async Task<TEntity?> UpdateAsync(TEntity entity)
  {
    entity.UpdatedDate = DateTime.Now;
    _context.Set<TEntity>().Update(entity);
    await _context.SaveChangesAsync();
    return entity;
  }
}
