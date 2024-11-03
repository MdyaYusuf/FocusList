using FocusList.Models.Enums;

namespace FocusList.Models.Dtos.ToDos.Requests;

public sealed record CreateToDoRequest(string Title, string Description, DateTime StartDate, DateTime EndDate, Priority Priority, bool IsCompleted, int CategoryId, string UserId);
