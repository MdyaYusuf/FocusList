using Core.Responses;
using FocusList.Models.Dtos.Tokens.Responses;
using FocusList.Models.Dtos.Users.Requests;

namespace FocusList.Service.Abstracts;

public interface IAuthenticationService
{
  Task<ReturnModel<TokenResponseDto>> LoginAsync(LoginRequest request);
  Task<ReturnModel<TokenResponseDto>> RegisterAsync(RegisterRequest request);
}
