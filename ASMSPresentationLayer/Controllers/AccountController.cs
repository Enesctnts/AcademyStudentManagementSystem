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
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                if (checkUserForEmail!=null)
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
                    BirthDate = model.BirthDate.HasValue ? model.BirthDate.Value : null, // HasValue null mı diye bakıyor.
                    Gender = model.Gender,
                    EmailConfirmed = true,
                    UserName = model.Email
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
                    return RedirectToAction("Login", "Account", new { email = model.Email });

                }
                else
                {
                    ModelState.AddModelError("", "Beklenmedik bir sorun oldu. Üye kaydı başarısız tekrar deneyiniz!");
                    return View(model);
                }


            }
            catch (Exception)
            {
                //loglanacak
                return RedirectToAction("Error", "Home");
            }
        }


    }
}
