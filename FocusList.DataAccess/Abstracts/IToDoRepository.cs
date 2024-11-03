using Core.Repositories;
using FocusList.Models.Entities;

namespace FocusList.DataAccess.Abstracts;

public interface IToDoRepository : IRepository<ToDo, Guid>
{
  Task<int> GetToDosCountByUserAsync(string userId);
  IQueryable<ToDo> GetByUserId(string userId);
}
