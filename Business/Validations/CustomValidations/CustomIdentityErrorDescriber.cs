using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validations.CustomValidations
{
	public class CustomIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = "DuplicateUserName", Description = $"\"{userName}\" başka bir kullanıcı tarafından kullanılmaktadır." };
		public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = "InvalidUserName", Description = "Lütfen kullanıcı adı oluştururken Türkçe karakter(ı, İ, ü, ğ, ş, ö, ç) kullanmayınız. Özel karakterlerden ise yalnızca birkaçını (-, ., _, @, +) kullanabilirsiniz." };
		public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = "DuplicateEmail", Description = $"\"{email}\" başka bir kullanıcı tarafından kullanılmaktadır." };
		public override IdentityError InvalidEmail(string email) => new IdentityError { Code = "InvalidEmail", Description = "Geçersiz email." };
	}
}
