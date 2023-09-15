using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;
using System.Drawing;

namespace HelpingHands_V2.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccount _account;

        public AccountController(IAccount account)
        {
            _account = account;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(model.Username) && string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "Username or Password were no filled in.");
                }

                if (await ValidateUser(model))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim(ClaimTypes.Role, "A"),
                        new Claim(ClaimTypes.PrimarySid, model.UserId.ToString()),

                    };

                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect("/");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Nurses");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }
            return View();
        }
    
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public async Task<bool> ValidateUser(LoginModel model)
        {
            var users =  await _account.GetUser(model.Username);
            var user = users.FirstOrDefault();

            if(user?.UserId > 0)
            {
                if(user.Username == model.Username && user.Password == model.Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
