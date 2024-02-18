using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc.Controllers
{
    public class AccountController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMapper mapper, IUserService userService, IMailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
            _mailService = mailService;
        }

        [AllowAnonymous]
		public IActionResult SignUp()
		{
            return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> SignUp(AppUserRegisterDto appUserRegisterDto)
		{        
     
            if (ModelState.IsValid)
			{
				AppUser appUser = new();
				appUser = _mapper.Map<AppUser>(appUserRegisterDto);
				appUser.RegisterDate = DateTime.Now;
                appUser.About = "Merhaba ben Politter kullanıyorum.";

                UploadPhotoService photoService = new();
                var newPicture = photoService.UploadProfilePicture(appUserRegisterDto.ProfilePicture);

                if (newPicture != null)
				{
                    appUser.ProfilePicture = newPicture;   
                }
				else
				{ //cartoons klasöründen rastgele atama yapılıyor
                    string[] filePaths = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/profilePictures/cartoons"));
                    Random random = new();
                    string newFilePath = filePaths[random.Next(filePaths.Count())];
                    //  dosya yolundan yerel dizin adresini remove ettim

                    int CurrentDirectoryLenght = Directory.GetCurrentDirectory().Length;
                    //  /wwwroot dizininden kurtulmak için +8 ekledim
                    //  böylelikle ProfilePicture ataması yaparken /img/profilePictures/cartoons/? dizinini kullabiliyorum
                    appUser.ProfilePicture = newFilePath.Remove(0, CurrentDirectoryLenght + 8);
                }

                var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);

				if (result.Succeeded)
				{
                    _mailService.SendMailForLogging(appUser);
					return RedirectToAction("SignIn", "Account");
				}
				else
					result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
			}

			return View();
		}

		[AllowAnonymous]
		public IActionResult SignIn(string ReturnUrl)
		{
            TempData["returnUrl"] = ReturnUrl;
            if (ReturnUrl == null) // Anasayfadan login olunursa Url null geliyor
			{
				TempData["returnUrl"] = "/Posts/Index";
            }
			
            return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn(AppUserLoginDto appUserLoginDto)
		{
			AppUser appUser = new();
			appUser = _mapper.Map<AppUser>(appUserLoginDto);

            //AppUser user = await _userManager.FindByNameAsync(appUser.UserName);
            //ViewData["user"] = user.FirstName + " " + user.LastName;

            if (ModelState.IsValid)
            {
                if (await _userManager.FindByNameAsync(appUser.UserName) != null)
                {
                    await _signInManager.SignOutAsync(); // for clean cookies
                    var result = await _signInManager.PasswordSignInAsync(appUserLoginDto.UserName, appUserLoginDto.Password, false, true);
                    if (result.Succeeded)
                    {
						//return RedirectToAction("Index", "Posts");						
                        return Redirect(TempData["returnUrl"].ToString());
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Kullanıcı adı veya şifre hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Böyle bir kullanıcı bulunmamaktadır!");
                }
            }
			return View();
		}

		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index","Home");
		}

        public async Task<IActionResult> EditProfile()
		{
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            AppUserDetailsDto userDto = _mapper.Map<AppUserDetailsDto>(user);
            return View(userDto);      
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(AppUserDetailsDto userDto)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                // null geldiği için burada geçici atama yapıldı
                userDto.ProfilePicture = user.ProfilePicture;

                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.Email = userDto.Email;
                user.PhoneNumber = userDto.PhoneNumber;
                user.UserName = userDto.UserName;
                user.About = userDto.About;

                UploadPhotoService photoService = new();
                var newPicture = photoService.UploadProfilePicture(userDto.UploadProfilePicture);
               
                if(newPicture != null) 
                {
                    // 3 stringi birleştirmediği için çözüm biraz uzadı
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    FileInfo oldPicture = new FileInfo($"{path}{user.ProfilePicture}");

                    user.ProfilePicture = newPicture;
                    userDto.ProfilePicture = user.ProfilePicture;
                    if (System.IO.File.Exists(oldPicture.ToString()))
                    {
                        oldPicture.Delete();
                    }   
                }

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                    return View(userDto);
                }

                ViewBag.message = "Profil güncellendi!";
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, true);
            }
            userDto.ProfilePicture = user.ProfilePicture;
            return View(userDto);
        }   

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
		public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
		{
            if(ModelState.IsValid)
            {
				AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

				if (await _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword))
				{
					IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
					if (!result.Succeeded)
					{
						result.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
						return View(changePasswordDto);
					}
					await _userManager.UpdateSecurityStampAsync(user);
					await _signInManager.SignOutAsync();
					await _signInManager.SignInAsync(user, true);
					return RedirectToAction(nameof(EditProfile));
				}
                else 
                { 
                    ViewBag.wrongPwd = "Eski parolanız yanlış!";
				}
			}
            return View();
		}

		[AllowAnonymous]
		public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

                if (user != null)
                {
                    string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
					passwordResetToken = HttpUtility.UrlEncode(passwordResetToken);

					var passwordResetTokenLink = Url.Action("ResetPassword", "Account", new
                    {
                        userId = user.Id,
                        token = passwordResetToken
                    }, HttpContext.Request.Scheme);

                    _mailService.SendMailForResetPassword(user, passwordResetTokenLink);

                    ViewBag.State = true;
                }
                else
                    ViewBag.State = false;

            }
            return View();
        }

		[HttpGet("[action]/{userId}/{token}")]
		[AllowAnonymous]
		public IActionResult ResetPassword()
		{
			return View();
		}

		[HttpPost("[action]/{userId}/{token}")]
		[AllowAnonymous]
		public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto, string userId, string token)
		{
			if (ModelState.IsValid)
			{
                AppUser user = await _userManager.FindByIdAsync(userId);

				IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), resetPasswordDto.NewPassword);
				
				if (result.Succeeded)
				{
					ViewBag.State = true;
					await _userManager.UpdateSecurityStampAsync(user);
				}
				else
					ViewBag.State = false;

			}
			return View();
		}

        public async Task<IActionResult> DeleteAccount()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Isdeleted = true;
			await _userManager.UpdateAsync(user);
			await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
	}
}