using ASMSBusinessLayer.ContractsBLL;
using ASMSBusinessLayer.EmailService;
using ASMSBusinessLayer.ViewModels;
using ASMSDataAccessLayer.ContactsDAL;
using ASMSEntityLayer.Enums;
using ASMSEntityLayer.IdentityModels;
using ASMSEntityLayer.ResultModels;
using ASMSEntityLayer.ViewModels;
using ASMSPresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IStudentBusinessEngine _studentBusinessEngine;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IEmailSender emailSender, IStudentBusinessEngine studentBusinessEngine)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _studentBusinessEngine = studentBusinessEngine;
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                // !ModelState.IsValid ==> burda gelen model var mı, data var mı diye bakıyoruz.
                if (!ModelState.IsValid)
                {
                    //return View(model);

                    TempData["RegisterFailedMessage"] = "Veri girişlerini istenildiği gibi yapamadık.Tekrar deneyiniz!";
                    return RedirectToAction("Index", "Home");
                }

                
                var checkUserForEmail = await _userManager.FindByIdAsync(model.Email);

                //Aynı emailden tekrar kayıt olunmasın 
                if (checkUserForEmail!=null)
                {
                    //ModelState.AddModelError("", "Bu email ile zaten sisteme kayıt yapılmıştır");
                    //return View(model);

                    TempData["RegisterFailedMessage"] = "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!";
                    return RedirectToAction("Index", "Home");
                }

                //User' ı oluşturalım
                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    BirthDate = model.BirthDate.HasValue ? model.BirthDate.Value : null, // HasValue null mı diye bakıyor.
                    Gender = model.Gender,
                    EmailConfirmed = true,
                    UserName = model.Email,
                    TCNumber=model.TCNumber
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded) //Eklendi
                {
                    //rol ataması
                    var roleResult = await _userManager.AddToRoleAsync(newUser,ASMSRoles.Student.ToString());
                    if (roleResult.Succeeded == false)
                    {
                        //Admine gizliden bir email gönder eklensin. Enum a Rolü ekleyip bu işlemi gerçekleştriebiliriz.
                    }

                    //Student eklensin
                    StudentVM newStudent = new StudentVM()
                    {
                        UserId = newUser.Id,
                        TCNumber = model.TCNumber
                    };

                    IResult resultStudent = _studentBusinessEngine.Add(newStudent);
                    if (resultStudent.IsSuccess == false)
                    {
                        //Admine gizliden bir email gönder eklensin öğrenciyi kayıt oldu ama sisteme kayıt edilirken sorun oldu öğrencinin bilgilerini admin gizliden mail atarak kayıt olmasını sağlayabiliriz.
                    }

                    //email gönedrilsin
                    var emailToStudent = new EmailMessage()
                    {
                        Subject = $"ASMS Sistemine HOŞ GELDİNİZ! {newUser.Name} {newUser.Surname}",
                        Body = $"Merhaba, Sisteme kaydınız gerçekleşmiştir...",

                        Contacts = new string[] { model.Email }
                    };
                    await _emailSender.SendMessage(emailToStudent);
                    TempData["RegisterSuccessMessage"] = "Sisteme kaydınız başarıyla gerçekleşti!";

                    return RedirectToAction("Login", "Account", new { email = model.Email });

                }
                else
                {
                    //ModelState.AddModelError("", "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!");
                    //return View(model);

                    TempData["RegisterFailedMessage"] = "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!";
                    return RedirectToAction("Login", "Account");
                }


            }
            catch (Exception)
            {
                //loglanacak
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Login(string email)
        {
            LoginViewModel model = new LoginViewModel()
            {
                Email = email
            };
            return View(model);
          
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = await _userManager.FindByNameAsync(model.Email);
                //Böyle bir kullanıcı var mı?
                if (user==null)
                {
                    ModelState.AddModelError("", "Epostanız yada şifreniz hatalıdı! Tekrar deneyiniz!");
                    return View();
                }
                //Kullanıcının parolası doğru mu?

                //ToDo: son parametre bool lockoutOnFailure ile ilgili bu da kaç defa şifre girdiğinde sisteme belirli süre şifre girişi yapamasın bunun için.
                var result = await _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe, false);

                //ToDo: son parametre bool lockoutOnFailure ile ilgili bu da kaç defa şifre girdiğinde sisteme belirli süre şifre girişi yapamasın bunun için.
                //Burda sistem lockoutOnFailure işlemi true mu false mu diye bakıyor. eger lockoutOnFailure true yaparsak şifreyi mesela 3 kez yanlış yazdıktan sonra belirli bir süre sisteme giriş yapamasın işleminin sonucunda sistem kilitli mi diye bakıyoruz.Sistem kilitliyse aşagıdaki hataayı verdircez
                //if (result.IsLockedOut)
                //{
                //    DateTimeOffset d = user.LockoutEnd.Value;
                //}

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Epostanız ya da şifreniz hatalıdır! Tekrar deneyiniz!");
                    return View();
                }
                //artık hoş geldi
                //Sisteme giren kişinin rolü Student ise Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user,ASMSRoles.Student.ToString()).Result)
                {
                    return RedirectToAction("Index", "Home");
                }
                //Sisteme giren kişinin rolü admin Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user, ASMSRoles.Coordinator.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                //Sisteme giren kişinin rolü öğrenci işleri Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user, ASMSRoles.StudentAdministration.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz");

                //ex loglanacak
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ViewBag.ResetPasswordMessage = "Şifre yenileme talebiniz alındı! Epostanızı kontrol ediniz!";
                    return View();
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var codeEncode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callBackUrl = Url.Action("ConfirmResetPassword", "Account", new { userId = user.Id, code = codeEncode }, protocol: Request.Scheme);
                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Subject="ASMS - Yeni Şifre Talebi",
                    Body=$"Merhaba {user.Name} {user.Surname},"  +
                    $"<br/> Yeni parola belirlemek için" +
                    $"<a href='{HtmlEncoder.Default.Encode(callBackUrl)}'> buraya </a> tıklayınız..."

                };

                await _emailSender.SendMessage(emailMessage);
                ViewBag.ResetPasswordSuccessMessage = "Şifre yenileme talebiniz alındı! Epostanızı kontrol ediniz!";
                return View();
            }
            catch (Exception ex)
            {

                // ex loglanacak
                ViewBag.ResetPasswordFailMessage = "Beklenmedik bir hata oluştu! Tekrar deneyiniz!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ConfirmResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                ViewBag.ConfirmResetPasswordFailureMessage = "Beklenmedik bir hata oluştu!";
                return View();
            }
            //Bu alttakileri bu şekilde cshtml sayfasına gönderebilirdik yukarda bu şekil yaptık ama yukarda tek bir tane değişken vardı onu viewbag ile gönderdik. burda 2 değişken var ViewModel ile göndermek bu ikisini daha mantıklı daha güzel durur.
            //ViewBag.UserId = userId;
            //ViewBag.Code = code;

            //yukardaki işlemi Viewmodel yaparak yapmış olduk.
            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                UserId = userId,
                Code = code
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user!=null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı");
                    //log mesajı yerleştir.
                    //throw new Exception();
                }
                //ResetPasswordViewModel daki code alanında anlamsız bir token değeri olcak onu string çevirip tokenDecoded e atıyoruz.
                var tokenDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));

                //burda ise şifre yenileme işlemini usermanagerın bize sunduğu ResetPasswordAsync metodu sayesinde yapıyoruz. Bu metoda user , token değerimizi ve yeni şifremizi yolluyoruz.
                var result = await _userManager.ResetPasswordAsync(user, tokenDecoded, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["ConfirmResetPasswordSuccess"] = "Şifreniz başarıyla güncellenmiştir!";
                    return RedirectToAction("login", "Account", new{email = user.Email});
                }
                else
                {
                    ModelState.AddModelError("", "Şifrenizin değiştirilme işleminde beklenmedik bir hata oluştu! Tekrar deneyiniz");
                    return View(model);
                }

            }
            catch (Exception ex)
            {

                //ex loglanacak
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz!");
                return View(model);

            }
        }
    }
}