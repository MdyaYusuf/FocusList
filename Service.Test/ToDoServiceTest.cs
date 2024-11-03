using AutoMapper;
using Core.Tokens.Services;
using FocusList.DataAccess.Abstracts;
using FocusList.Models.Dtos.ToDos.Requests;
using FocusList.Models.Dtos.ToDos.Responses;
using FocusList.Models.Entities;
using FocusList.Models.Enums;
using FocusList.Service.Concretes;
using FocusList.Service.Rules;
using Moq;

namespace Service.Test;

public class ToDoServiceTest
{
  private Mock<IToDoRepository> _todoRepositoryMock;
  private Mock<IMapper> _mapperMock;
  private Mock<ToDoBusinessRules> _businessRulesMock;
  private Mock<DecoderService> _decoderServiceMock;
  private ToDoService _todoService;

  [SetUp]
  public void Setup()
  {
    _todoRepositoryMock = new Mock<IToDoRepository>();
    _mapperMock = new Mock<IMapper>();
    _businessRulesMock = new Mock<ToDoBusinessRules>();
    _decoderServiceMock = new Mock<DecoderService>();
    _todoService = new ToDoService(_todoRepositoryMock.Object, _mapperMock.Object, _businessRulesMock.Object, _decoderServiceMock.Object);
  }

  [Test]
  public async Task AddAsync_ShouldReturnSuccess_WhenToDoIsAdded()
  {
    // Arrange
    var request = new CreateToDoRequest
    (
      Title: "NewToDo",
      Description: "Description",
      StartDate: DateTime.Now,
      EndDate: DateTime.Now.AddDays(1),
      Priority: Priority.Medium,
      IsCompleted: false,
      CategoryId: 1,
      UserId: "user123"
    );

    var createdToDo = new ToDo()
    {
      Id = Guid.NewGuid(),
      Title = "NewToDo",
      Description = "Description",
      StartDate = request.StartDate,
      EndDate = request.EndDate,
      Priority = request.Priority
    };

    var responseDto = new ToDoResponseDto()
    {
      Title = createdToDo.Title,
      Description = createdToDo.Description,
      StartDate = createdToDo.StartDate,
      EndDate = createdToDo.EndDate,
      Priority = createdToDo.Priority
    };

    _businessRulesMock.Setup(x => x.ValidateDates(request.StartDate, request.EndDate));
    _businessRulesMock.Setup(x => x.CheckMaxToDosPerUserAsync(request.UserId)).Returns(Task.CompletedTask);
    _mapperMock.Setup(x => x.Map<ToDo>(request)).Returns(createdToDo);
    _todoRepositoryMock.Setup(x => x.AddAsync(createdToDo)).Returns((Task<ToDo>)Task.CompletedTask);
    _mapperMock.Setup(x => x.Map<ToDoResponseDto>(createdToDo)).Returns(responseDto);

    // Act
    var result = await _todoService.AddAsync(request);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Yapılacak iş eklendi.", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(201, result.StatusCode);
  }

  [Test]
  public async Task GetAllAsync_ShouldReturnListOfToDos()
  {
    // Arrange
    var todos = new List<ToDo>()
    {
      new ToDo() 
      {
        Id = Guid.NewGuid(),
        Title = "FirstToDo",
        Description = "FirstDesc",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(1),
        Priority = Priority.Medium
      },
      new ToDo()
      { 
        Id = Guid.NewGuid(),
        Title = "SecondaryToDo",
        Description = "SecondaryDesc",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(2),
        Priority = Priority.Low
      }
    };
    var responseDtos = new List<ToDoResponseDto>()
    {
      new ToDoResponseDto { Title = todos[0].Title, Description = todos[0].Description, StartDate = todos[0].StartDate, EndDate = todos[0].EndDate, Priority = todos[0].Priority },
      new ToDoResponseDto { Title = todos[1].Title, Description = todos[1].Description, StartDate = todos[1].StartDate, EndDate = todos[1].EndDate, Priority = todos[1].Priority }
    };

    _todoRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(todos);
    _mapperMock.Setup(x => x.Map<List<ToDoResponseDto>>(todos)).Returns(responseDtos);

    // Act
    var result = await _todoService.GetAllAsync();

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Yapılacak işler listesi başarılı bir şekilde getirildi.", result.Message);
    Assert.AreEqual(responseDtos, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task GetByIdAsync_ShouldReturnToDo_WhenToDoExists()
  {
    // Arrange
    var todoId = Guid.NewGuid();
    var todo = new ToDo()
    {
      Id = todoId,
      Title = "ToDo",
      Description = "Description",
      StartDate = DateTime.Now,
      EndDate = DateTime.Now.AddDays(1),
      Priority = Priority.Medium
    };
    var responseDto = new ToDoResponseDto()
    {
      Title = todo.Title,
      Description = todo.Description,
      StartDate = todo.StartDate,
      EndDate = todo.EndDate,
      Priority = todo.Priority
    };

    _businessRulesMock.Setup(x => x.IsToDoExistAsync(todoId)).Returns(Task.CompletedTask);
    _todoRepositoryMock.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todo);
    _mapperMock.Setup(x => x.Map<ToDoResponseDto>(todo)).Returns(responseDto);

    // Act
    var result = await _todoService.GetByIdAsync(todoId);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual($"{todoId} numaralı yapılacak iş başarılı bir şekilde getirildi.", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task GetToDosByUserAsync_ShouldReturnUserSpecificToDos()
  {
    // Arrange
    var userId = "user123";
    var todos = new List<ToDo>()
    {
        new ToDo()
        {
          Id = Guid.NewGuid(),
          Title = "FirstUserToDo",
          Description = "FirstDesc",
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddDays(1),
          UserId = userId,
          Priority = Priority.Medium
        },
        new ToDo()
        {
          Id = Guid.NewGuid(),
          Title = "SecondaryUserToDo",
          Description = "SecondaryDesc",
          StartDate = DateTime.Now,
          EndDate = DateTime.Now.AddDays(2),
          UserId = userId,
          Priority = Priority.Low
        }
    };
    var responseDtos = todos.Select(todo => new ToDoResponseDto()
    {
      Title = todo.Title,
      Description = todo.Description,
      StartDate = todo.StartDate,
      EndDate = todo.EndDate,
      Priority = todo.Priority
    }).ToList();

    _decoderServiceMock.Setup(x => x.GetUserId()).Returns(userId);
    _todoRepositoryMock.Setup(x => x.GetByUserId(userId)).Returns(todos.AsQueryable());
    _mapperMock.Setup(x => x.Map<IQueryable<ToDoResponseDto>>(todos)).Returns(responseDtos.AsQueryable());

    // Act
    var result = await _todoService.GetToDosByUserAsync();

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Kullanıcıya özel ToDo listesi başarılı bir şekilde getirildi.", result.Message);
    Assert.AreEqual(responseDtos.AsQueryable(), result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task RemoveAsync_ShouldReturnSuccess_WhenToDoIsRemoved()
  {
    // Arrange
    var todoId = Guid.NewGuid();
    var todo = new ToDo()
    {
      Id = todoId, Title = "ToDo",
      Description = "Description"
    };
    var deletedToDo = new ToDo()
    {
      Id = todoId,
      Title = "ToDo",
      Description = "Description"
    };
    var responseDto = new ToDoResponseDto()
    {
      Title = todo.Title,
      Description = todo.Description
    };

    _businessRulesMock.Setup(x => x.IsToDoExistAsync(todoId)).Returns(Task.CompletedTask);
    _todoRepositoryMock.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todo);
    _todoRepositoryMock.Setup(x => x.RemoveAsync(todo)).ReturnsAsync(deletedToDo);
    _mapperMock.Setup(x => x.Map<ToDoResponseDto>(deletedToDo)).Returns(responseDto);

    // Act
    var result = await _todoService.RemoveAsync(todoId);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Yapılacak iş başarılı bir şekilde silindi", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }

  [Test]
  public async Task UpdateAsync_ShouldReturnSuccess_WhenToDoIsUpdated()
  {
    // Arrange
    var request = new UpdateToDoRequest
    (
      Id: Guid.NewGuid(),
      Title: "UpdatedToDo",
      Description: "UpdatedDescription",
      StartDate: DateTime.Now,
      EndDate: DateTime.Now.AddDays(1),
      Priority: Priority.High,
      IsCompleted: false,
      UserId: "user123"
    );

    var existingToDo = new ToDo()
    {
      Id = request.Id,
      Title = "OldToDo",
      Description = "OldDescription"
    };
    var updatedToDo = new ToDo()
    {
      Id = request.Id,
      Title = request.Title,
      Description = request.Description,
      StartDate = request.StartDate,
      EndDate = request.EndDate,
      Priority = request.Priority
    };
    var responseDto = new ToDoResponseDto()
    {
      Title = updatedToDo.Title,
      Description = updatedToDo.Description,
      StartDate = updatedToDo.StartDate,
      EndDate = updatedToDo.EndDate,
      Priority = updatedToDo.Priority
    };

    _businessRulesMock.Setup(x => x.IsToDoExistAsync(request.Id)).Returns(Task.CompletedTask);
    _todoRepositoryMock.Setup(x => x.GetByIdAsync(request.Id)).ReturnsAsync(existingToDo);
    _todoRepositoryMock.Setup(x => x.UpdateAsync(existingToDo)).ReturnsAsync(updatedToDo);
    _mapperMock.Setup(x => x.Map<ToDoResponseDto>(updatedToDo)).Returns(responseDto);

    // Act
    var result = await _todoService.UpdateAsync(request);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.AreEqual("Yapılacak iş güncellendi.", result.Message);
    Assert.AreEqual(responseDto, result.Data);
    Assert.AreEqual(200, result.StatusCode);
  }
}
