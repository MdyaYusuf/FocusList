using Core.Exceptions;
using FocusList.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace FocusList.Service.Rules;

public class UserBusinessRules(UserManager<User> _userManager)
{
  public async Task<User> EnsureUserExistsAsync(string id)
  {
    var user = await _userManager.FindByIdAsync(id);

    if (user == null)
    {
      throw new NotFoundException("Kullanıcı bulunamadı.");
    }

    return user;
  }

  public async Task<User> EnsureUserExistsByEmailAsync(string email)
  {
    var user = await _userManager.FindByEmailAsync(email);

    if (user == null)
    {
      throw new NotFoundException("Kullanıcı bulunamadı.");
    }

    return user;
  }

  public void EnsurePasswordsMatch(string newPassword, string confirmNewPassword)
  {
    if (!newPassword.Equals(confirmNewPassword))
    {
      throw new BusinessException("Parolalar uyuşmuyor.");
    }
  }

  public async Task IsUsernameUniqueAsync(string username)
  { 
    var existingUser = await _userManager.FindByNameAsync(username);

    if (existingUser != null)
    {
      throw new BusinessException($"{username} kullanıcı adı daha önceden alınmış.");
    }
  }
}
