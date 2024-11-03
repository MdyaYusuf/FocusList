using FocusList.DataAccess.Abstracts;
using FocusList.DataAccess.Concretes;
using FocusList.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FocusList.DataAccess;

public static class DataAccessDependencies
{
  public static IServiceCollection AddDataAccessDependencies(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IToDoRepository, EfToDoRepository>();
    services.AddScoped<ICategoryRepository, EfCategoryRepository>();
    services.AddDbContext<BaseDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
    return services;
  }
}
