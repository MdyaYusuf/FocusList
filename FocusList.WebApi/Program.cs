using FocusList.DataAccess;
using FocusList.DataAccess.Contexts;
using Core.Tokens.Configurations;
using Core.Tokens.Services;
using FocusList.Models.Entities;
using FocusList.Service;
using FocusList.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Core.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddDataAccessDependencies(builder.Configuration);
builder.Services.AddServiceDependencies();

builder.Services.AddScoped<DecoderService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<ToDoSettings>(builder.Configuration.GetSection("ToDoSettings"));

builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
  opt.User.RequireUniqueEmail = true;
  opt.Password.RequireNonAlphanumeric = false;
  opt.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<BaseDbContext>();

var tokenOption = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();

builder.Services.AddAuthentication(opt =>
{
  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
  opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = tokenOption.Issuer,
    ValidAudience = tokenOption.Audience[0],
    IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(tokenOption.SecurityKey)
  };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(_ => { });

app.MapControllers();

app.Run();
