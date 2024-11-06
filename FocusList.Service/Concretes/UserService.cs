using Azure.Core;
using Core.Exceptions;
using FocusList.Models.Dtos.Users.Requests;
using FocusList.Models.Entities;
using FocusList.Service.Abstracts;
using FocusList.Service.Rules;
using Microsoft.AspNetCore.Identity;

namespace FocusList.Service.Concretes;

public sealed class UserService : IUserService
{
  private readonly UserManager<User> _userManager;
  private readonly UserBusinessRules _userBusinessRules;
  public UserService(UserManager<User> userManager, UserBusinessRules userBusinessRules)
  {
    _userManager = userManager;
    _userBusinessRules = userBusinessRules;
  }

  public async Task<User> ChangePasswordAsync(string id, ChangePasswordRequest request)
  {
    var user = await _userBusinessRules.EnsureUserExistsAsync(id);
    _userBusinessRules.EnsurePasswordsMatch(request.NewPassword, request.ConfirmNewPassword);

    var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

    CheckForIdentityResult(result);

    return user;
  }

  public async Task<string> DeleteAsync(string id)
  {
    var user = await _userBusinessRules.EnsureUserExistsAsync(id);

    var result = await _userManager.DeleteAsync(user);

    CheckForIdentityResult(result);

    return "Kullanıcı silindi.";
  }

  public async Task<User> GetByEmailAsync(string email)
  {
    var user = await _userBusinessRules.EnsureUserExistsByEmailAsync(email);

    return user;
  }

  public async Task<User> LoginAsync(LoginRequest request)
  {
    var user = await _userBusinessRules.EnsureUserExistsByEmailAsync(request.Email);

    bool checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

    if (checkPassword == false)
    {
      throw new BusinessException("Parolanız yanlış.");
    }

    return user;
  }

  public async Task<User> RegisterAsync(RegisterRequest request)
  {
    await _userBusinessRules.IsUsernameUniqueAsync(request.Username);

    User user = new User
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      City = request.City,
      UserName = request.Username,
    };

    var result = await _userManager.CreateAsync(user, request.Password);

    CheckForIdentityResult(result);

    var addRole = await _userManager.AddToRoleAsync(user, "User");

    CheckForIdentityResult(result);

    return user;
  }

  public async Task<User> UpdateAsync(string id, UserUpdateRequest request)
  {
    var user = await _userBusinessRules.EnsureUserExistsAsync(id);
    await _userBusinessRules.IsUsernameUniqueAsync(request.Username);

    user.UserName = request.Username;
    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.City = request.City;

    var result = await _userManager.UpdateAsync(user);
    CheckForIdentityResult(result);

    return user;
  }

  private void CheckForIdentityResult(IdentityResult result)
  {
    if (!result.Succeeded)
    {
      throw new BusinessException(result.Errors.ToList().First().Description);
    }
  }
}
