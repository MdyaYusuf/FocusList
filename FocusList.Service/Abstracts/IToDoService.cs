using Core.Responses;
using FocusList.Models.Dtos.ToDos.Requests;
using FocusList.Models.Dtos.ToDos.Responses;

namespace FocusList.Service.Abstracts;

public interface IToDoService
{
  Task<ReturnModel<List<ToDoResponseDto>>> GetAllAsync();
  Task<ReturnModel<ToDoResponseDto?>> GetByIdAsync(Guid id);
  Task<ReturnModel<ToDoResponseDto>> AddAsync(CreateToDoRequest request);
  Task<ReturnModel<ToDoResponseDto>> UpdateAsync(UpdateToDoRequest request);
  Task<ReturnModel<ToDoResponseDto>> RemoveAsync(Guid id);
  Task<ReturnModel<IQueryable<ToDoResponseDto>>> GetToDosByUserAsync();
}
