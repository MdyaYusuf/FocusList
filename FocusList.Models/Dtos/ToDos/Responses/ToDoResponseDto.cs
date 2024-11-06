using FocusList.Models.Entities;
using FocusList.Models.Enums;

namespace FocusList.Models.Dtos.ToDos.Responses;

public sealed record ToDoResponseDto
{
  public string Title { get; init; } = default!;
  public string Description { get; init; } = default!;
  public DateTime StartDate { get; init; }
  public DateTime EndDate { get; init; }
  public Priority Priority { get; init; }
  public string Category { get; init; } = default!;
  public string UserName { get; init; } = default!;
}
