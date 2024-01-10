using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations.UserValidations
{
	public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
	{
        public ResetPasswordDtoValidator()
        {
			RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni parola boş olamaz!").MinimumLength(5).WithMessage("Parola en az 5 karakter olmalı!");
			RuleFor(x => x.PasswordConfirm).NotEmpty().WithMessage("Parola doğrulaması boş olamaz!").MinimumLength(5).WithMessage("Parola en az 5 karakter olmalı!");
			RuleFor(x => x.NewPassword).Equal(x => x.PasswordConfirm).WithMessage("Girilen yeni parola ve doğrulama aynı değil!");
		}
    }
}
