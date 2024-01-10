using Core.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business.Validations.UserValidations
{
    public class AppUserDetailsDtoValidator : AbstractValidator<AppUserDetailsDto>
    {
        public AppUserDetailsDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim alanı boş bırakılamaz");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir E-mail adresi giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail boş bırakılamaz");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Bir kullanıcı adı girin");
            //RuleFor(x => x.PhoneNumber).Matches(new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")).WithMessage("Telefonunuzu Başında 0 olmadan 10 hane olarak giriniz");
            RuleFor(x => x.PhoneNumber).MaximumLength(15).WithMessage("Telefon numarası Çok uzun olmadı mı? :)");
        }
    }
}
