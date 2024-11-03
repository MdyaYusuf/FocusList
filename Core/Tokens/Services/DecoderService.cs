using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Tokens.Services;

public class DecoderService(IHttpContextAccessor httpContextAccessor)
{
  public string GetUserId()
  {
    return httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
  }
}
