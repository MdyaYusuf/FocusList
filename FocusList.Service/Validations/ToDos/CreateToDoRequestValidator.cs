using FluentValidation;
using FocusList.Models.Dtos.ToDos.Requests;

namespace FocusList.Service.Validations.ToDos;

public class CreateToDoRequestValidator : AbstractValidator<CreateToDoRequest>
{
  public CreateToDoRequestValidator()
  {
    RuleFor(t => t.Title).NotEmpty().WithMessage("ToDo başlığı boş bırakılamaz.")
      .Length(2, 50).WithMessage("ToDo başlığı minimum 2, maksimum 50 karakterli olmalıdır.");

    RuleFor(t => t.Description).NotEmpty().WithMessage("Description içeriği boş bırakılamaz.");

    RuleFor(t => t.Priority).IsInEnum().WithMessage("Geçerli bir Priority değeri girmelisiniz.");

    RuleFor(t => t.CategoryId).GreaterThan(0).WithMessage("Kategori Id'si sıfırdan büyük olmalıdır.");
  }
}
