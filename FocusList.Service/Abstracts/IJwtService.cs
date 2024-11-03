using FocusList.Models.Dtos.Tokens.Responses;
using FocusList.Models.Entities;

namespace FocusList.Service.Abstracts;

public interface IJwtService
{
  Task<TokenResponseDto> CreateJwtTokenAsync(User user);
}
