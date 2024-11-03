using Core.Repositories;
using FocusList.DataAccess.Abstracts;
using FocusList.DataAccess.Contexts;
using FocusList.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FocusList.DataAccess.Concretes;

public class EfCategoryRepository : EfBaseRepository<BaseDbContext, Category, int>, ICategoryRepository
{
  public EfCategoryRepository(BaseDbContext context) : base(context)
  {
    
  }

  public async Task<Category?> GetByNameAsync(string name)
  {
    return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
  }
}
