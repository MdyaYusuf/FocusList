using FluentValidation;
using FocusList.Models.Dtos.ToDos.Requests;

namespace FocusList.Service.Validations.ToDos;

public class UpdateToDoRequestValidator : AbstractValidator<UpdateToDoRequest>
{
  public UpdateToDoRequestValidator()
  {
    RuleFor(t => t.Id)
      .NotEmpty().WithMessage("ToDo Id boş bırakılamaz.");

    RuleFor(t => t.Title).NotEmpty().WithMessage("ToDo başlığı boş bırakılamaz.")
      .Length(2, 50).WithMessage("ToDo başlığı minimum 2, maksimum 50 karakterli olmalıdır.");

    RuleFor(t => t.Description).NotEmpty().WithMessage("Description içeriği boş bırakılamaz.");

    RuleFor(t => t.Priority).IsInEnum().WithMessage("Geçerli bir Priority değeri girmelisiniz.");
  }
}
