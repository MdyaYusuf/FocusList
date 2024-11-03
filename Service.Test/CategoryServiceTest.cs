using AutoMapper;
using FocusList.DataAccess.Abstracts;
using FocusList.Models.Dtos.Categories.Requests;
using FocusList.Models.Dtos.Categories.Responses;
using FocusList.Models.Entities;
using FocusList.Service.Concretes;
using FocusList.Service.Rules;
using Moq;

namespace Service.Test;

public class CategoryServiceTest
{
  private CategoryService _categoryService;
  private Mock<ICategoryRepository> _categoryRepositoryMock;
  private Mock<IMapper> _mapperMock;
  private Mock<CategoryBusinessRules> _businessRulesMock;

  [SetUp]
  public void Setup()
  {
    _categoryRepositoryMock = new Mock<ICategoryRepository>();
    _mapperMock = new Mock<IMapper>();
    _businessRulesMock = new Mock<CategoryBusinessRules>();
    _categoryService = new CategoryService(_categoryRepositoryMock.Object, _mapperMock.Object, _businessRulesMock.Object);
  }

  [Test]
  public async Task AddAsync_ShouldReturnSuccess_WhenCategoryIsAdded()
  {
    // Arrange
    var request = new CreateCategoryRequest("NewCategory");
    var createdCategory = new Category
    {
      Id = 1,
      Name = "NewCategory"
    };
    var responseDto = new CategoryResponseDto()
    {
      Id = 1,
      Name = "NewCategory"
    };

    _businessRulesMock.Setup(x => x.IsNameUnique(request.Name)).Returns(Task.CompletedTask);
    _mapperMock.Setup(x => x.Map<Category>(request)).Returns(createdCategory);
    _categoryRepositoryMock.Setup(x => x.AddAsync(createdCategory)).Returns((Task<Category>)Task.CompletedTask);
    _mapperMock.Setup(x => x.Map<CategoryResponseDto>(createdCategory)).Returns(responseDto);

    // Act
    var result = await _categoryService.AddAsync(request);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Kategori eklendi", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(201, result.StatusCode);
  }

  [Test]
  public async Task GetAllAsync_ShouldReturnListOfCategories()
  {
    // Arrange
    var categories = new List<Category>()
    {
      new Category { Id = 1, Name = "FirstCategory" },
      new Category { Id = 2, Name = "SecondCategory" }
    };

    var responseDtos = new List<CategoryResponseDto>()
    {
      new CategoryResponseDto { Id = 1, Name = "FirstCategory" },
      new CategoryResponseDto { Id = 2, Name = "SecondCategory" }
    };

    _categoryRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(categories);
    _mapperMock.Setup(x => x.Map<List<CategoryResponseDto>>(categories)).Returns(responseDtos);

    // Act
    var result = await _categoryService.GetAllAsync();

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Kategori listesi başarılı bir şekilde getirildi.", result.Message);
    Assert.AreEqual(responseDtos, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
  {
    // Arrange
    int categoryId = 1;
    var category = new Category()
    {
      Id = categoryId,
      Name = "FirstCategory"
    };

    var responseDto = new CategoryResponseDto()
    {
      Id = categoryId, Name = "FirstCategory"
    };

    _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
    _mapperMock.Setup(x => x.Map<CategoryResponseDto>(category)).Returns(responseDto);

    // Act
    var result = await _categoryService.GetByIdAsync(categoryId);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual($"{categoryId} numaralı kategori başarılı bir şekilde getirildi.", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task RemoveAsync_ShouldReturnSuccess_WhenCategoryIsRemoved()
  {
    // Arrange
    int categoryId = 1;
    var category = new Category()
    {
      Id = categoryId,
      Name = "FirstCategory"
    };
    var deletedCategory = new Category()
    {
      Id = categoryId,
      Name = "FirstCategory"
    };
    var responseDto = new CategoryResponseDto()
    {
      Id = categoryId,
      Name = "FirstCategory"
    };

    _businessRulesMock.Setup(x => x.IsCategoryExistAsync(categoryId)).Returns(Task.CompletedTask);
    _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId)).ReturnsAsync(category);
    _categoryRepositoryMock.Setup(x => x.RemoveAsync(category)).ReturnsAsync(deletedCategory);
    _mapperMock.Setup(x => x.Map<CategoryResponseDto>(deletedCategory)).Returns(responseDto);

    // Act
    var result = await _categoryService.RemoveAsync(categoryId);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Kategori başarılı bir şekilde silindi", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task UpdateAsync_ShouldReturnSuccess_WhenCategoryIsUpdated()
  {
    // Arrange
    var request = new UpdateCategoryRequest(1, "UpdatedCategory");
    var existingCategory = new Category()
    {
      Id = request.Id,
      Name = "OldCategory"
    };
    var updatedCategory = new Category()
    {
      Id = request.Id,
      Name = request.Name
    };
    var responseDto = new CategoryResponseDto()
    {
      Id = request.Id,
      Name = request.Name
    };

    _businessRulesMock.Setup(x => x.IsCategoryExistAsync(request.Id)).Returns(Task.CompletedTask);
    _categoryRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(existingCategory);
    _categoryRepositoryMock.Setup(x => x.UpdateAsync(existingCategory)).ReturnsAsync(updatedCategory);
    _mapperMock.Setup(x => x.Map<CategoryResponseDto>(updatedCategory)).Returns(responseDto);

    // Act
    var result = await _categoryService.UpdateAsync(request);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Kategori güncellendi.", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }
}
