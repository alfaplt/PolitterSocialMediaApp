using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations.UserValidations
{
    public class AppUserRegisterDtoValidator : AbstractValidator<AppUserRegisterDto>
    {
        public AppUserRegisterDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş bırakılamaz");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir E-mail adresi giriniz");
			RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail boş bırakılamaz");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Bir kullanıcı adı girin");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Bir parola girin").MinimumLength(5).WithMessage("Parola en az 5 karakter olmalı");
            RuleFor(x => x.PasswordConfirm).Equal(x => x.Password).WithMessage("Girilen parolalar aynı değil");

		}
    }
}
