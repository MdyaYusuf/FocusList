using FocusList.Models.Dtos.Users.Requests;

namespace FocusList.Service.Abstracts;

public interface IRoleService
{
  Task<string> AddRoleToUser(AddRoleToUserRequest request);
  Task<List<string>> GetAllRolesByUserId(string userId);
  Task<string> AddRoleAsync(string name);
}
