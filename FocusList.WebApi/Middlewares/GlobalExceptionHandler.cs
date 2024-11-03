using Core.Exceptions;
using Core.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace FocusList.WebApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    ReturnModel<List<string>> Errors = new ReturnModel<List<string>>();
    httpContext.Response.ContentType = "application/json";
    httpContext.Response.StatusCode = 500;

    if (exception.GetType() == typeof(NotFoundException))
    {
      httpContext.Response.StatusCode = 404;
      Errors.Success = false;
      Errors.Message = exception.Message;
      Errors.StatusCode = 404;

      await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

      return true;
    }

    if (exception.GetType() == typeof(BusinessException))
    {
      httpContext.Response.StatusCode = 400;
      Errors.Success = false;
      Errors.Message = exception.Message;
      Errors.StatusCode = 400;

      await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

      return true;
    }

    Errors.Success = false;
    Errors.Message = exception.Message;
    Errors.StatusCode = 500;

    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(Errors));

    return true;

  }
}
