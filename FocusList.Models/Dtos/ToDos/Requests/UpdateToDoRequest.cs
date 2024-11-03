using FocusList.Models.Enums;

namespace FocusList.Models.Dtos.ToDos.Requests;

public sealed record UpdateToDoRequest(Guid Id, string Title, string Description, DateTime StartDate, DateTime EndDate, Priority Priority, bool IsCompleted, string UserId);
