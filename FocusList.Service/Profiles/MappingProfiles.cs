using AutoMapper;
using FocusList.Models.Dtos.Categories.Requests;
using FocusList.Models.Dtos.Categories.Responses;
using FocusList.Models.Dtos.ToDos.Requests;
using FocusList.Models.Dtos.ToDos.Responses;
using FocusList.Models.Entities;

namespace FocusList.Service.Profiles;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    CreateMap<CreateCategoryRequest, Category>();
    CreateMap<UpdateCategoryRequest, Category>();
    CreateMap<Category, CategoryResponseDto>();

    CreateMap<CreateToDoRequest, ToDo>();
    CreateMap<UpdateToDoRequest, ToDo>();
    CreateMap<ToDo, ToDoResponseDto>();
  }
}
