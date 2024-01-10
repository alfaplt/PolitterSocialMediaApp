using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations.UserValidations
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("Kullanıcı adı boş olamaz");
            RuleFor(x => x.Password).NotNull().WithMessage("Şifreni girmeyi mi unuttun :)");
        }
    }
}
