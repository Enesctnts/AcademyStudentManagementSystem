using ASMSBusinessLayer.EmailService;
using ASMSBusinessLayer.ViewModels;
using ASMSEntityLayer.Enums;
using ASMSEntityLayer.IdentityModels;
using ASMSPresentationLayer.Areas.Management.Models;
using ASMSPresentationLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Areas.Management.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IEmailSender emailSender, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAdminViewModel model)
        {
            try
            {
                // !ModelState.IsValid ==> burda gelen model var mı, data var mı diye bakıyoruz.
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var checkUserForEmail = await _userManager.FindByIdAsync(model.Email);

                //Aynı emailden tekrar kayıt olunmasın 
                if (checkUserForEmail != null)
                {
                    ModelState.AddModelError("", "Bu email ile zaten sisteme kayıt yapılmıştır");
                    return View(model);
                }

                //User' ı oluşturalım
                AppUser newUser = new AppUser()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    EmailConfirmed = true,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded) //Eklendi
                {
                    //rol ataması
                    var roleResult = await _userManager.AddToRoleAsync(newUser, ASMSRoles.StudentAdministration.ToString());
                    if (roleResult.Succeeded == false)
                    {
                        //Admine gizliden bir email gönder eklensin. Enum a Rolü ekleyip bu işlemi gerçekleştriebiliriz.
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
                    _logger.LogInformation("Sisteme yeni bir öğrenci işleri personeli kayıt oldu. userId" + newUser.Id );
                    return RedirectToAction("Login", "Management/Admin", new {email = model.Email });

                }
                else
                {
                    
                    ModelState.AddModelError("", "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!");
                    return View(model);

                }

            }
            catch (Exception ex)
            {
                //loglanacak
                _logger.LogError($"Management/Admin Register Hata oldu" + ex.Message.ToString());
                ModelState.AddModelError("", "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!");
                return View(model);
            }
        }


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
                if (user == null)
                {
                    ModelState.AddModelError("", "Epostanız yada şifreniz hatalıdı! Tekrar deneyiniz!");
                    return View(model);
                }
                //Kullanıcının parolası doğru mu?

                //ToDo: son parametre bool lockoutOnFailure ile ilgili bu da kaç defa şifre girdiğinde sisteme belirli süre şifre girişi yapamasın bunun için.
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                //ToDo: son parametre bool lockoutOnFailure ile ilgili bu da kaç defa şifre girdiğinde sisteme belirli süre şifre girişi yapamasın bunun için.
                //Burda sistem lockoutOnFailure işlemi true mu false mu diye bakıyor. eger lockoutOnFailure true yaparsak şifreyi mesela 3 kez yanlış yazdıktan sonra belirli bir süre sisteme giriş yapamasın işleminin sonucunda sistem kilitli mi diye bakıyoruz.Sistem kilitliyse aşagıdaki hataayı verdircez
                //if (result.IsLockedOut)
                //{
                //    DateTimeOffset d = user.LockoutEnd.Value;
                //}

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Epostanız ya da şifreniz hatalıdır! Tekrar deneyiniz!");
                    return View(model);
                }
                //artık hoş geldi
                //Sisteme giren kişinin rolü Student ise Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user, ASMSRoles.Student.ToString()).Result)
                {
                    return RedirectToAction("Index", "Home");
                }
                //Sisteme giren kişinin rolü admin Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user, ASMSRoles.Coordinator.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Management/Admin");
                }
                //Sisteme giren kişinin rolü öğrenci işleri Dashboard sayfası açılsın diyoruz.
                if (_userManager.IsInRoleAsync(user, ASMSRoles.StudentAdministration.ToString()).Result)
                {
                    return RedirectToAction("Dashboard", "Management/Admin");
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

    }
}
