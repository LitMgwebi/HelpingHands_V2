﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpingHands_V2.Models;
using BC = BCrypt.Net.BCrypt;
using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

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

                ViewBag.User = user;
                return View();
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
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }

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
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
        public async Task<IActionResult> Register([Bind("Username, Firstname, Lastname, DateOfBirth, Email, Password,ConfirmPassword, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user, string? returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                //var fileName = Path.GetFileName(file.FileName);

                //user.ProfilePictureName = fileName;
                //using (var target = new MemoryStream())
                //{
                //    file.CopyTo(target);
                //    user.ProfilePicture = target.ToArray();
                //}

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
                        if (user.UserType == "A")
                            return RedirectToAction("Dashboard", "Admin");
                        else if (user.UserType == "N")
                            return RedirectToAction("Create", "Nurse", new { id = user.UserId });
                        else if (user.UserType == "P")
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
                //return new JsonResult(new { error = ex.Message });
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

                ViewBag.User = user;
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
        public async Task<IActionResult> Edit([Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                // If Role = Admin, return to Index. else if role = P || M return to profile
                user.Password = BC.HashPassword(user.Password);
                await _account.UpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [HttpGet]
        public IActionResult ChangePassword(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                ViewBag.UserId = id;
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
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel change)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                var user = await _account.GetUserById(change.UserId);
                if (BC.Verify(change.CurrentPassword, user.Password))
                {
                    user.Password = BC.HashPassword(change.NewPassword);
                    await _account.UpdateUser(user);

                    if (HttpContext.User.IsInRole("P"))
                        return RedirectToAction("Profile", "Patient", new { id = change.UserId });
                    else if (HttpContext.User.IsInRole("N"))
                        return RedirectToAction("Profile", "Nurse", new { id = change.UserId });
                    else if (HttpContext.User.IsInRole("O"))
                        return RedirectToAction("Profile", "EndUser", new { id = change.UserId });
                    else
                        return RedirectToAction("Profile", "EndUser", new { id = change.UserId });

                }
                return new JsonResult(new { error = "Password not changes", change, user });
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
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
