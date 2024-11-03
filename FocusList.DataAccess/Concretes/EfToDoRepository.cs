using Core.Repositories;
using FocusList.DataAccess.Abstracts;
using FocusList.DataAccess.Contexts;
using FocusList.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusList.DataAccess.Concretes;

public class EfToDoRepository : EfBaseRepository<BaseDbContext, ToDo, Guid>, IToDoRepository
{
  public EfToDoRepository(BaseDbContext context) : base(context)
  {
    
  }

  public async Task<int> GetToDosCountByUserAsync(string userId)
  {
    return await _context.ToDos.CountAsync(todo => todo.UserId == userId);
  }

  public IQueryable<ToDo> GetByUserId(string userId)
  {
    return _context.ToDos.Where(todo => todo.UserId == userId);
  }
}
