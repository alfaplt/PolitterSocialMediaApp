using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations.UserValidations
{
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().WithMessage("Email giriniz!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email adresini uygun formatta giriniz");
        }
    }
}
