namespace FocusList.Models.Dtos.Categories.Responses;

public sealed record CategoryResponseDto
{
  public int Id { get; set; }
  public string Name { get; init; } = default!;
}