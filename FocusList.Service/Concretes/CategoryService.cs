using AutoMapper;
using Core.Responses;
using FocusList.DataAccess.Abstracts;
using FocusList.Models.Dtos.Categories.Requests;
using FocusList.Models.Dtos.Categories.Responses;
using FocusList.Models.Entities;
using FocusList.Service.Abstracts;
using FocusList.Service.Rules;

namespace FocusList.Service.Concretes;

public class CategoryService(ICategoryRepository _categoryRepository, IMapper _mapper, CategoryBusinessRules _businessRules) : ICategoryService
{
  public async Task<ReturnModel<CategoryResponseDto>> AddAsync(CreateCategoryRequest request)
  {
    await _businessRules.IsNameUnique(request.Name);

    Category createdCategory = _mapper.Map<Category>(request);
    await _categoryRepository.AddAsync(createdCategory);
    CategoryResponseDto response = _mapper.Map<CategoryResponseDto>(createdCategory);

    return new ReturnModel<CategoryResponseDto>()
    {
      Success = true,
      Message = "Kategori eklendi",
      Data = response,
      StatusCode = 201
    };
  }

  public async Task<ReturnModel<List<CategoryResponseDto>>> GetAllAsync()
  {
    List<Category> categories = await _categoryRepository.GetAllAsync();
    List<CategoryResponseDto> responseList = _mapper.Map<List<CategoryResponseDto>>(categories);

    return new ReturnModel<List<CategoryResponseDto>>()
    {
      Success = true,
      Message = "Kategori listesi başarılı bir şekilde getirildi.",
      Data = responseList,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<CategoryResponseDto?>> GetByIdAsync(int id)
  {
    Category? category = await _categoryRepository.GetByIdAsync(id);
    CategoryResponseDto? response = _mapper.Map<CategoryResponseDto>(category);

    return new ReturnModel<CategoryResponseDto?>()
    {
      Success = true,
      Message = $"{id} numaralı kategori başarılı bir şekilde getirildi.",
      Data = response,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<CategoryResponseDto>> RemoveAsync(int id)
  {
    await _businessRules.IsCategoryExistAsync(id);

    Category category = await _categoryRepository.GetByIdAsync(id);
    Category deletedCategory = await _categoryRepository.RemoveAsync(category);
    CategoryResponseDto response = _mapper.Map<CategoryResponseDto>(deletedCategory);

    return new ReturnModel<CategoryResponseDto>()
    {
      Success = true,
      Message = "Kategori başarılı bir şekilde silindi",
      Data = response,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<CategoryResponseDto>> UpdateAsync(UpdateCategoryRequest request)
  {
    await _businessRules.IsCategoryExistAsync(request.Id);

    Category existingCategory = await _categoryRepository.GetByIdAsync(request.Id);

    existingCategory.Id = existingCategory.Id;
    existingCategory.Name = request.Name;

    Category updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
    CategoryResponseDto dto = _mapper.Map<CategoryResponseDto>(updatedCategory);

    return new ReturnModel<CategoryResponseDto>()
    {
      Success = true,
      Message = "Kategori güncellendi.",
      Data = dto,
      StatusCode = 200
    };
  }
}
