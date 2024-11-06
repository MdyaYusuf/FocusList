using Core.Exceptions;
using FocusList.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace FocusList.Service.Rules;

public class RoleBusinessRules
{
  private readonly UserManager<User> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  public RoleBusinessRules(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
  {
    _userManager = userManager;
    _roleManager = roleManager;
  }

  public void EnsureUserExist(User user)
  {
    if (user == null)
    {
      throw new NotFoundException("Kullanıcı bulunamadı.");
    }
  }

  public void EnsureRoleExist(IdentityRole role)
  {
    if (role == null)
    {
      throw new BusinessException("Rol bulunamadı.");
    }
  }

  public async Task IsRoleUniqueAsync(string roleName)
  {
    var role = await _roleManager.FindByNameAsync(roleName);

    if (role != null)
    {
      throw new BusinessException("Eklemek istediğiniz rol benzersiz olmalıdır.");
    }
  }
}
