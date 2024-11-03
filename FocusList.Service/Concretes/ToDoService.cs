using AutoMapper;
using Core.Responses;
using Core.Tokens.Services;
using FocusList.DataAccess.Abstracts;
using FocusList.Models.Dtos.ToDos.Requests;
using FocusList.Models.Dtos.ToDos.Responses;
using FocusList.Models.Entities;
using FocusList.Service.Abstracts;
using FocusList.Service.Rules;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FocusList.Service.Concretes;

public class ToDoService : IToDoService
{
  private readonly IToDoRepository _todoRepository;
  private readonly IMapper _mapper;
  private readonly ToDoBusinessRules _businessRules;
  private readonly DecoderService _decoderService;
  public ToDoService(IToDoRepository todoRepository, IMapper mapper, ToDoBusinessRules businessRules, DecoderService decoderService)
  {
    _todoRepository = todoRepository;
    _mapper = mapper;
    _businessRules = businessRules;
    _decoderService = decoderService;
  }

  public async Task<ReturnModel<ToDoResponseDto>> AddAsync(CreateToDoRequest request)
  {
    _businessRules.ValidateDates(request.StartDate, request.EndDate);
    await _businessRules.CheckMaxToDosPerUserAsync(request.UserId);

    ToDo createdToDo = _mapper.Map<ToDo>(request);
    await _todoRepository.AddAsync(createdToDo);
    ToDoResponseDto response = _mapper.Map<ToDoResponseDto>(createdToDo);

    return new ReturnModel<ToDoResponseDto>()
    {
      Success = true,
      Message = "Yapılacak iş eklendi.",
      Data = response,
      StatusCode = 201
    };
  }

  public async Task<ReturnModel<List<ToDoResponseDto>>> GetAllAsync()
  {
    List<ToDo> todos = await _todoRepository.GetAllAsync();
    List<ToDoResponseDto> responseList = _mapper.Map<List<ToDoResponseDto>>(todos);

    return new ReturnModel<List<ToDoResponseDto>>()
    {
      Success = true,
      Message = "Yapılacak işler listesi başarılı bir şekilde getirildi.",
      Data = responseList,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<ToDoResponseDto?>> GetByIdAsync(Guid id)
  {
    await _businessRules.IsToDoExistAsync(id);

    ToDo? todo = await _todoRepository.GetByIdAsync(id);
    ToDoResponseDto? response = _mapper.Map<ToDoResponseDto>(todo);

    return new ReturnModel<ToDoResponseDto?>()
    {
      Success = true,
      Message = $"{id} numaralı yapılacak iş başarılı bir şekilde getirildi.",
      Data = response,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<IQueryable<ToDoResponseDto>>> GetToDosByUserAsync()
  {
    string userId = _decoderService.GetUserId();
    var query = _todoRepository.GetByUserId(userId);
    var toDos = await query.ToListAsync();
    var responseList = _mapper.Map<IQueryable<ToDoResponseDto>>(toDos);

    return new ReturnModel<IQueryable<ToDoResponseDto>>() 
    { 
      Success = true,
      Message = "Kullanıcıya özel ToDo listesi başarılı bir şekilde getirildi.",
      Data = responseList,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<ToDoResponseDto>> RemoveAsync(Guid id)
  {
    await _businessRules.IsToDoExistAsync(id);

    ToDo todo = await _todoRepository.GetByIdAsync(id);
    ToDo deletedToDo = await _todoRepository.RemoveAsync(todo);
    ToDoResponseDto response = _mapper.Map<ToDoResponseDto>(deletedToDo);

    return new ReturnModel<ToDoResponseDto>()
    {
      Success = true,
      Message = "Yapılacak iş başarılı bir şekilde silindi",
      Data = response,
      StatusCode = 200
    };
  }

  public async Task<ReturnModel<ToDoResponseDto>> UpdateAsync(UpdateToDoRequest request)
  {
    await _businessRules.IsToDoExistAsync(request.Id);

    ToDo existingToDo = await _todoRepository.GetByIdAsync(request.Id);

    existingToDo.Id = existingToDo.Id;
    existingToDo.Title = request.Title;
    existingToDo.Description = request.Description;
    existingToDo.StartDate = request.StartDate;
    existingToDo.EndDate = request.EndDate;
    existingToDo.Priority = request.Priority;
    existingToDo.IsCompleted = request.IsCompleted;
    existingToDo.UserId = request.UserId;

    ToDo updatedToDo = await _todoRepository.UpdateAsync(existingToDo);
    ToDoResponseDto dto = _mapper.Map<ToDoResponseDto>(updatedToDo);

    return new ReturnModel<ToDoResponseDto>()
    {
      Success = true,
      Message = "Yapılacak iş güncellendi.",
      Data = dto,
      StatusCode = 200
    };
  }
}
