using ASMSBusinessLayer.ContractsBLL;
using ASMSBusinessLayer.EmailService;
using ASMSEntityLayer.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASMSPresentationLayer.Controllers
{

    [Authorize]
    public class AddressController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IUsersAddressBusinessEngine _userAdress;

        public AddressController(UserManager<AppUser> userManager, IEmailSender emailSender, IUsersAddressBusinessEngine userAdress)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _userAdress = userAdress;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            return View();
        }
    }
}

