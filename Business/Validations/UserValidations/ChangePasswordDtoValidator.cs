using Core.DTOs;
using FluentValidation;

namespace Business.Validations.UserValidations
{
	public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
	{
        public ChangePasswordDtoValidator()
        {
			RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Eski parola boş olamaz!");
			RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni parola boş olamaz!").MinimumLength(5).WithMessage("Parola en az 5 karakter olmalı!");
			RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage("Parola doğrulaması boş olamaz!").MinimumLength(5).WithMessage("Parola en az 5 karakter olmalı!");
			RuleFor(x => x.NewPassword).Equal(x => x.PasswordConfirm).WithMessage("Girilen yeni parola ve doğrulama aynı değil!");
			RuleFor(x => x.OldPassword).NotEqual(x => x.NewPassword).WithMessage("Eski ve yeni parola için aynı değerleri girdin!");
		}
    }
}
