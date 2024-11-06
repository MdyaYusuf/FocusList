using FluentValidation.AspNetCore;
using FluentValidation;
using FocusList.Service.Abstracts;
using FocusList.Service.Concretes;
using FocusList.Service.Profiles;
using FocusList.Service.Rules;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FocusList.Service;

public static class ServiceDependencies
{
  public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
  {
    services.AddAutoMapper(typeof(MappingProfiles));
    services.AddScoped<ToDoBusinessRules>();
    services.AddScoped<CategoryBusinessRules>();
    services.AddScoped<UserBusinessRules>();
    services.AddScoped<RoleBusinessRules>();
    services.AddScoped<IJwtService, JwtService>();
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IToDoService, ToDoService>();
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<IRoleService, RoleService>();
    services.AddFluentValidationAutoValidation();
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    return services;
  }
}
