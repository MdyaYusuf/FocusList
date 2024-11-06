namespace FocusList.Models.Dtos.Users.Requests;

public sealed record AddRoleToUserRequest(string UserId, string RoleName);
