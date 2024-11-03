using Core.Repositories;
using FocusList.Models.Entities;

namespace FocusList.DataAccess.Abstracts;

public interface ICategoryRepository : IRepository<Category, int>
{
  Task<Category?> GetByNameAsync(string name);
}
