using Core.Exceptions;
using FocusList.Models.Dtos.Users.Requests;
using FocusList.Models.Entities;
using FocusList.Service.Abstracts;
using FocusList.Service.Rules;
using Microsoft.AspNetCore.Identity;

namespace FocusList.Service.Concretes;

public class RoleService : IRoleService
{
  private readonly UserManager<User> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly RoleBusinessRules _roleBusinessRules;
  public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, RoleBusinessRules roleBusinessRules)
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _roleBusinessRules = roleBusinessRules;
  }

  public async Task<string> AddRoleAsync(string name)
  {
    await _roleBusinessRules.IsRoleUniqueAsync(name);

    var role = new IdentityRole()
    {
      Name = name
    };

    var result = await _roleManager.CreateAsync(role);

    if (!result.Succeeded)
    {
      throw new BusinessException(result.Errors.First().Description);
    }

    return $"{name} isimli rol eklendi: ";
  }

  public async Task<string> AddRoleToUser(AddRoleToUserRequest request)
  {
    var role = await _roleManager.FindByNameAsync(request.RoleName);
    _roleBusinessRules.EnsureRoleExist(role);

    var user = await _userManager.FindByIdAsync(request.UserId);
    _roleBusinessRules.EnsureUserExist(user);

    var addRoleToUser = await _userManager.AddToRoleAsync(user, request.RoleName);

    if (!addRoleToUser.Succeeded)
    {
      throw new BusinessException(addRoleToUser.Errors.First().Description);
    }

    return $"Kullanıcıya {request.RoleName} isimli rol eklendi: ";
  }

  public async Task<List<string>> GetAllRolesByUserId(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    _roleBusinessRules.EnsureUserExist(user);

    var roles = await _userManager.GetRolesAsync(user);

    return roles.ToList();
  }
}
