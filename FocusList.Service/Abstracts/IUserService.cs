using FocusList.Models.Dtos.Users.Requests;
using FocusList.Models.Entities;

namespace FocusList.Service.Abstracts;

public interface IUserService
{
  Task<User> RegisterAsync(RegisterRequest request);
  Task<User> GetByEmailAsync(string email);
  Task<User> LoginAsync(LoginRequest request);
  Task<User> UpdateAsync(string id, UserUpdateRequest request);
  Task<string> DeleteAsync(string id);
  Task<User> ChangePasswordAsync(string id, ChangePasswordRequest request);
}
