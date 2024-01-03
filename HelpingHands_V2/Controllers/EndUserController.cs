using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpingHands_V2.Models;
using BC = BCrypt.Net.BCrypt;
using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using HelpingHands_V2.ViewModels;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HelpingHands_V2.Services;

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
                //ViewBag.Accounts = accounts;
                return View(accounts);

            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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

                //ViewBag.User = user;
                return View(user);
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                var api_key = Environment.GetEnvironmentVariable("API_KEY");
                var api_secret = Environment.GetEnvironmentVariable("API_SECRET");

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    //return new JsonResult(new { errors });
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }

                if (string.IsNullOrEmpty(model.Username) && string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.Message = "Username or Password were not filled in.";
                    return View();
                }
                if (await ValidateUser(model))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username!),
                        new Claim("FullName", model.FullName),
                        new Claim(ClaimTypes.Role, model.UserType!),
                        new Claim("UserId", model.UserId.ToString()),
                        new Claim("Username", model.Username!)

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
                    //return new JsonResult( new {model, message = "could not validate user"});
                    ViewBag.Message = "Could not validate user. Username or Password is incorrect";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
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
                IEnumerable<string> genders = new List<string> { "Male", "Female", "Non-Binary", "Gender-fluid", "Other" };

                ViewData["Genders"] = new SelectList(genders);
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username, Firstname, Lastname, DateOfBirth, Email, Password,ConfirmPassword, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user, IFormFile file, string? returnUrl = null)
        {
            List<SelectListItem> genders = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Male"},
                 new SelectListItem{ Value="2",Text="Female"},
                 new SelectListItem{ Value="3",Text="Non-Binary"},
                 new SelectListItem{ Value="4",Text="Gender-fluid"},
                 new SelectListItem{ Value="5",Text="Other"},
             };
            genders = data.ToList();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewData["Genders"] = new SelectList(genders);
                    return View();
                }
                if (file.Length > 0)
                {
                    CloudinaryService cloudinary = new CloudinaryService();

                    var public_id = $"{user.Firstname.ToLower()}_{user.Lastname.ToUpper()}_{user.DateOfBirth.Day}-{user.DateOfBirth.Month}-{user.DateOfBirth.Year}";
                    UploadResult uploadResult = await cloudinary.UploadToCloudinary(file, public_id);

                    user.ProfilePicture = uploadResult.SecureUrl.ToString();
                    user.ProfilePictureName = uploadResult.PublicId;

                }
                else
                {
                    throw new Exception("No file were uploaded onto system");
                }
                string tempPassword = user.Password;
                user.Password = BC.HashPassword(user.Password);
                await _account.AddUser(user);
                ViewBag.Message = "Record Added successfully;";

                if (await RegisterValidateUser(user, tempPassword))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username!),
                        new Claim("FullName", user.FullName),
                        new Claim(ClaimTypes.Role, user.UserType!),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Username", user.Username!)

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
                        if (user.UserType == "N")
                            return RedirectToAction("Create", "Nurse", new { id = user.UserId });
                        else if (user.UserType == "P")
                            return RedirectToAction("Create", "Patient", new { id = user.UserId });
                        else
                            return RedirectToAction("Dashboard", "Admin");
                    }
                }


                ViewBag.Message = "User is not validated";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Genders"] = new SelectList(genders);
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "A")]
        public IActionResult AddUser()
        {
            try
            {
                IEnumerable<string> genders = new List<string> { "Male", "Female", "Non-Binary", "Gender-fluid", "Other" };

                ViewData["Genders"] = new SelectList(genders);
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser([Bind("Username, Firstname, Lastname, DateOfBirth, Email, Password,ConfirmPassword, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    IEnumerable<string> genders = new List<string> { "Male", "Female", "Non-Binary", "Gender-fluid", "Other" };

                    ViewData["Genders"] = new SelectList(genders);
                    return View();
                }

                string tempPassword = user.Password;
                user.Password = BC.HashPassword(user.Password);
                await _account.AddUser(user);
                ViewBag.Message = "Record Added successfully;";
                if (await RegisterValidateUser(user, tempPassword))
                {
                    if (user.UserType == "N")
                        return RedirectToAction("Create", "Nurse", new { id = user.UserId });
                    else
                        return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return new JsonResult(new { error = "User is not validated" });
                }

            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                IEnumerable<string> genders = new List<string> { "Male", "Female", "Non-Binary", "Gender-fluid", "Other" };

                ViewData["Genders"] = new SelectList(genders);
                ViewBag.Message = ex.Message;
                return View();
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

                IEnumerable<string> genders = new List<string> { "Male", "Female", "Non-Binary", "Gender-fluid", "Other" };

                ViewData["Genders"] = new SelectList(genders);
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user, IFormFile file)
        {
            List<SelectListItem> genders = new List<SelectListItem>();
            var data = new[]{
                 new SelectListItem{ Value="1",Text="Male"},
                 new SelectListItem{ Value="2",Text="Female"},
                 new SelectListItem{ Value="3",Text="Non-Binary"},
                 new SelectListItem{ Value="4",Text="Gender-fluid"},
                 new SelectListItem{ Value="5",Text="Other"},
             };
            genders = data.ToList();
            try
            {
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("Password");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";

                    ViewData["Genders"] = new SelectList(genders);
                    return View(user);
                }
                if (file.Length > 0)
                {
                    CloudinaryService cloudinary = new CloudinaryService();

                    if (user.ProfilePictureName != null)
                    {
                        var removalResult = await cloudinary.RemoveFromCloudinary(user.ProfilePictureName!);
                    }
                    var public_id = $"{user.Firstname.ToLower()}_{user.Lastname.ToUpper()}_{user.DateOfBirth.Day}-{user.DateOfBirth.Month}-{user.DateOfBirth.Year}";
                    UploadResult uploadResult = await cloudinary.UploadToCloudinary(file, public_id);

                    user.ProfilePicture = uploadResult.SecureUrl.ToString();
                    user.ProfilePictureName = uploadResult.PublicId;
                }
                // If Role = Admin, return to Index. else if role = P || M return to profile
                await _account.UpdateUser(user);

                if (user.UserType == "A")
                    return RedirectToAction("Profile", "EndUser", new { id = user.UserId });
                else if (user.UserType == "N")
                    return RedirectToAction("Profile", "Nurse", new { id = user.UserId });
                else if (user.UserType == "P")
                    return RedirectToAction("Profile", "Patient", new { id = user.UserId });
                else
                    return RedirectToAction("Profile", "EndUser", new { id = user.UserId });
            }
            catch (Exception ex)
            {
                ViewData["Genders"] = genders;
                ViewBag.Message = ex.Message;
                return View(user);
            }
        }


        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            try
            {

                //ViewBag.UserId = id;
                ChangePasswordViewModel change = new ChangePasswordViewModel();
                change.UserId = id;

                return View(change);
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewBag.UserId = changePassword.UserId;
                    return View(changePassword);
                }
                var user = await _account.GetUserById(changePassword.UserId);
                if (BC.Verify(changePassword.CurrentPassword, user.Password))
                {
                    user.Password = BC.HashPassword(changePassword.NewPassword);
                    await _account.UpdateUser(user);

                    if (HttpContext.User.IsInRole("P"))
                        return RedirectToAction("Profile", "Patient", new { id = changePassword.UserId });
                    else if (HttpContext.User.IsInRole("N"))
                        return RedirectToAction("Profile", "Nurse", new { id = changePassword.UserId });
                    else if (HttpContext.User.IsInRole("O"))
                        return RedirectToAction("Profile", "EndUser", new { id = changePassword.UserId });
                    else
                        return RedirectToAction("Profile", "EndUser", new { id = changePassword.UserId });

                }
                ViewBag.Message = "Password not changed";
                return View(changePassword);
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View(changePassword);
            }
        }


        // Sql query to soft delete Nurse/Patient with the same userId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("UserId")] int UserId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Profile), new { id = UserId });
                }
                await _account.DeleteUser(UserId);
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Profile), new { id = UserId });
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<bool> ValidateUser(LoginModel model)
        {
            var user = await _account.GetUserByUsername(model.Username!);

            if (user?.UserId > 0)
            {
                if (user.Username == model.Username && BC.Verify(model.Password, user.Password))
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

        public async Task<bool> RegisterValidateUser(EndUser user, string password)
        {
            var account = await _account.GetUserByUsername(user.Username!);

            if (account?.UserId > 0)
            {
                if (account.Username == user.Username && BC.Verify(password, account.Password))
                {
                    user.UserId = account.UserId;
                    return true;
                }
            }
            return false;
        }
    }
}
