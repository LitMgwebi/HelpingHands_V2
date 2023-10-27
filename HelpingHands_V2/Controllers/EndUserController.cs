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

        public async Task<IActionResult> Index()
        {
            try
            {
                var accounts = await _account.GetUsers();

                if (accounts == null)
                {
                    return NotFound();
                }
                ViewBag.Accounts = accounts;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _account.GetUserById(id);

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

                    if (await ValidateUser(model))
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

        public IActionResult Register()
        {
            try
            {
                return View();
            } catch (Exception ex) { return new JsonResult(new { error = ex.Message }); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user, string? returnUrl = null)
        {
            try
            {
                if (user.ApplicationType == "A" || user.ApplicationType == "P")
                {
                    user.UserType = user.ApplicationType;
                    await _account.AddUser(user);
                } else
                {
                    await _account.AddUser(user);
                }
                ViewBag.Message = "Record Added successfully;";

                if (await RegisterValidateUser(user))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username!),
                        new Claim(ClaimTypes.Role, user.ApplicationType!),
                        new Claim("FullName", user.FullName),
                        new Claim("UserId", user.UserId.ToString()),

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
                        if (user.ApplicationType == "A")
                            return RedirectToAction("Dashboard", "Admin");
                        else if (user.ApplicationType == "N")
                            return RedirectToAction("Create", "Nurse", new { id = user.UserId });
                        else if (user.ApplicationType == "P")
                            return RedirectToAction("Create", "Patient", new { id = user.UserId });
                        else
                            return RedirectToAction("Dashboard", "Manager");
                    }
                }
                else
                {
                    return new JsonResult(new { error = "User is not validated" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
                //ViewBag.Message = "Operation unsuccessful";
                //return View();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _account.GetUserById(id);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                // If Role = Admin, return to Index. else if role = P || M return to profile
                await _account.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return new JsonResult(new { error = ex.Message });
            }
        }

        // Sql query to soft delete Nurse/Patient with the same userId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("UserId")] int UserId)
        {
            try
            {
                await _account.DeleteUser(UserId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<bool> ValidateUser(LoginModel model)
        {
            var user =  await _account.GetUserByUsername(model.Username!);

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

        public async Task<bool> RegisterValidateUser(EndUser model)
        {
            var user = await _account.GetUserByUsername(model.Username!);

            if (user?.UserId > 0)
            {
                if (user.Username == model.Username && user.Password == model.Password)
                {
                    model.UserId = user.UserId;
                    return true;
                }
            }
            return false;
        }
    }
}
