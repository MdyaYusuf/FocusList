using FocusList.Service.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FocusList.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService _userService) : ControllerBase
{
  [HttpGet("email")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> GetByEmailAsync([FromQuery] string email)
  {
    var result = await _userService.GetByEmailAsync(email);
    return Ok(result);
  }
}
