using Core.Responses;
using FocusList.Models.Dtos.Categories.Requests;
using FocusList.Models.Dtos.Categories.Responses;

namespace FocusList.Service.Abstracts;

public interface ICategoryService
{
  Task<ReturnModel<List<CategoryResponseDto>>> GetAllAsync();
  Task<ReturnModel<CategoryResponseDto?>> GetByIdAsync(int id);
  Task<ReturnModel<CategoryResponseDto>> AddAsync(CreateCategoryRequest request);
  Task<ReturnModel<CategoryResponseDto>> UpdateAsync(UpdateCategoryRequest request);
  Task<ReturnModel<CategoryResponseDto>> RemoveAsync(int id);
}
