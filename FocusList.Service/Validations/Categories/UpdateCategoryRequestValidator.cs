using FluentValidation;
using FocusList.Models.Dtos.Categories.Requests;

namespace FocusList.Service.Validations.Categories;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
  public UpdateCategoryRequestValidator()
  {
    RuleFor(c => c.Id)
      .GreaterThan(0).WithMessage("Kategori Id'si sıfırdan büyük olmalıdır.");

    RuleFor(c => c.Name)
      .NotEmpty().WithMessage("Kategori isim alanı boş bırakılamaz.")
      .Length(2, 50).WithMessage("Kategori ismi minimum 2, maksimum 50 karakter olmalıdır.")
      .Matches(@"^[a-zA-Z\s]*$").WithMessage("Kategori ismi özel karakterler ve sayılar içeremez.");
  }
}
