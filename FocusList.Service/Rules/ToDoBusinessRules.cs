using Core.Exceptions;
using FocusList.DataAccess.Abstracts;

namespace FocusList.Service.Rules;

public class ToDoBusinessRules(IToDoRepository _toDoRepository)
{
  public async Task IsToDoExistAsync(Guid id)
  {
    var todo = await _toDoRepository.GetByIdAsync(id);

    if (todo == null)
    {
      throw new NotFoundException($"{id} numaralı yapılacak iş bulunamadı.");
    }
  }

  public void ValidateDates(DateTime startDate, DateTime endDate)
  {
    if (endDate < startDate)
    {
      throw new BusinessException("Yapılacak işin başlangıç tarihi bitiş tarihinden geç olamaz.");
    }
  }

  private const int maxToDos = 5;

  public async Task CheckMaxToDosPerUserAsync(string userId)
  {
    var userToDosCount = await _toDoRepository.GetToDosCountByUserAsync(userId);

    if (userToDosCount > maxToDos)
    {
      throw new BusinessException("Bir kullanıcı maksimum 5 işe sahip olabilir.");
    }
  }
}
