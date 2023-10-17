using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;

namespace HelpingHands_V2.Controllers
{
    public class EndUserController : Controller
    {
        private readonly IEndUser _account;

        public EndUserController(IEndUser account)
        {
            _account = account;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = _account.GetUserById(id);

                if (user == null)
                    return NotFound();

                ViewBag.User = user;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (string.IsNullOrEmpty(model.Username) && string.IsNullOrEmpty(model.Password))
                    {
                        ModelState.AddModelError("", "Username or Password were not filled in.");
                    }

                    if (ValidateUser(model))
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username!),
                        new Claim("FullName", model.FullName),
                        new Claim(ClaimTypes.Role, model.UserType!),
                        new Claim("UserId", model.UserId.ToString()),

                    };

                        var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                        HttpContext.User.AddIdentity(claimsIdentity);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect("/");
                        }
                        else
                        {
                            if (model.UserType == "A")
                                return RedirectToAction("Dashboard", "Admin");
                            else if (model.UserType == "N")
                                return RedirectToAction("Dashboard", "Nurse", new { id = model.UserId });
                            else if (model.UserType == "P")
                                return RedirectToAction("Dashboard", "Patient", new { id = model.UserId });
                            else
                                return RedirectToAction("Dashboard", "Manager");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Password is incorrect");
                    }
                }
            } catch(Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
            return View();
        }
    
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public bool ValidateUser(LoginModel model)
        {
            var user =  _account.GetUserByUsername(model.Username!);

            if (user?.UserId > 0)
            {
                if(user.Username == model.Username && user.Password == model.Password)
                {

                    model.DateOfBirth = user.DateOfBirth;
                    model.Firstname = user.Firstname;
                    model.Lastname = user.Lastname;
                    model.Email = user.Email;
                    model.UserId = user.UserId;
                    model.UserType = user.UserType;
                    return true;
                }
            }
            return false;
        }
    }
}
