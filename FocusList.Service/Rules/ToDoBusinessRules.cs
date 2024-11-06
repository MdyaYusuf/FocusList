using Core.Exceptions;
using Core.Settings;
using FocusList.DataAccess.Abstracts;
using Microsoft.Extensions.Options;

namespace FocusList.Service.Rules;

public class ToDoBusinessRules
{
  private readonly IToDoRepository _todoRepository;
  private readonly IOptions<ToDoSettings> _settings;
  public ToDoBusinessRules(IToDoRepository todoRepository, IOptions<ToDoSettings> settings)
  {
    _todoRepository = todoRepository;
    _settings = settings;
  }

  public async Task IsToDoExistAsync(Guid id)
  {
    var todo = await _todoRepository.GetByIdAsync(id);

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

  public async Task CheckMaxToDosPerUserAsync(string userId)
  {
    var userToDosCount = await _todoRepository.GetToDosCountByUserAsync(userId);

    if (userToDosCount > _settings.Value.MaxToDos)
    {
      throw new BusinessException($"Bir kullanıcı maksimum {_settings.Value.MaxToDos} işe sahip olabilir.");
    }
  }
}
