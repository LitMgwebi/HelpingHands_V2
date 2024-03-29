﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using HelpingHands_V2.ViewModels;
using CloudinaryDotNet.Actions;
using HelpingHands_V2.Services;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "N, A, O, W")]
    public class NurseController : Controller
    {
        private readonly Grp0444HelpingHandsContext _context;
        private readonly INurse _nurse;
        private readonly IReport _report;
        private readonly IEndUser _user;
        public List<SelectListItem> genders = new List<SelectListItem>
        {
            new SelectListItem("Male", "Male"),
            new SelectListItem("Female", "Female"),
            new SelectListItem("Non-Binary", "Non-Binary"),
            new SelectListItem("Gender-fluid", "Gender-fluid"),
            new SelectListItem("Other", "Other"),
        };

        public NurseController(Grp0444HelpingHandsContext context, INurse nurse, IReport report, IEndUser user)
        {
            _nurse = nurse;
            _context = context;
            _report = report;
            _user = user;
        }

        [Authorize(Roles = "N")]
        public async Task<IActionResult> Dashboard(int id)
        {
            try
            {
                List<CareContract> assignedContracts = new List<CareContract>();
                List<Visit> contractVisits = new List<Visit>();
                DateTime currentDate = DateTime.Now;
                assignedContracts = await _report.NurseAssignedContracts(id);
                foreach (var contract in assignedContracts)
                {
                    contractVisits = await _report.ContractVisits(contract.ContractId);

                    foreach (var visit in contractVisits)
                    {
                        if ((DateTime.Compare((DateTime)visit!.VisitDate!, currentDate)) > 0)
                            contract.Visits.Add(visit);
                    }
                }
                return View(assignedContracts);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [Authorize(Roles = "A, O")]
        public async Task<IActionResult> Index(string? command)
        {
            try
            {
                IEnumerable<Nurse> nurses;
                if (command == "waiting")
                    nurses = await _nurse.GetNursesWaiting();
                else
                    nurses = await _nurse.GetNurses();

                if (nurses == null)
                {
                    return NotFound();
                }
                return View(nurses);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "N, A, O")]
        public async Task<IActionResult> Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nurse = await _nurse.GetNurse(id);
                if (nurse == null)
                {
                    return NotFound();
                }
                return View(nurse);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "W, A")]
        public IActionResult Create(EndUser user)
        {
            try
            {
                Nurse nurse = new Nurse();
                nurse.EndUser = user;
                nurse.NurseId = user.UserId;
                return View(nurse);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseId,Grade,Active")] Nurse nurse, [Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                ModelState.Remove("NurseNavigation");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    nurse.EndUser = user;
                    return View(nurse);
                }
                await _nurse.AddNurse(nurse);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Pending", "Home");
            }
            catch (Exception ex)
            {
                nurse.EndUser = user;
                ViewBag.Message = ex.Message;
                return View(nurse);
            }
        }


        [Authorize(Roles = "N, A, O")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nurse = await _nurse.GetNurse(id);
                var user = await _user.GetUserById(id);

                if (nurse == null || user == null)
                {
                    return NotFound();
                }

                ViewData["Genders"] = genders;
                nurse.EndUser = user;
                return View(nurse);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("NurseId,Grade,Active")] Nurse nurse, IFormFile? file, [Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("Password");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewData["Genders"] = genders;
                    return View(nurse);
                }
                if (file != null)
                {
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
                }
                await _nurse.UpdateNurse(nurse);
                await _user.UpdateUser(user);
                return RedirectToAction(nameof(Profile), new { id = nurse.NurseId });
            }
            catch (Exception ex)
            {
                ViewData["Genders"] = genders;
                ViewBag.Message = ex.Message;
                return View(nurse);
            }
        }


        [Authorize(Roles = "N, A, O")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("NurseId")] int NurseId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Profile), new { id = NurseId });
                }
                await _nurse.DeleteNurse(NurseId);
                await _user.DeleteUser(NurseId);
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Profile), new { id = NurseId });
            }
        }


        [Authorize(Roles = "N")]
        public IActionResult VisitRange()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisitRange(int NurseId, DateTime StartDate, DateTime EndDate)
        {
            IEnumerable<VisitRange> visitRange = new List<VisitRange> { };
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                visitRange = await _report.NurseVisitRange(NurseId, StartDate, EndDate);
                return View(visitRange);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }


        [Authorize(Roles = "N")]
        public async Task<IActionResult> Visits(int id, string command)
        {
            IEnumerable<VisitRange> visits = new List<VisitRange> { };
            try
            {
                DateTime currentDate = DateTime.Now;
                if (command == "upcoming")
                {
                    visits = await _report.NurseVisitRange(id, currentDate, currentDate.AddYears(20));
                }
                else if (command == "past")
                {
                    visits = await _report.NurseVisitRange(id, currentDate.AddYears(-20), currentDate);
                }
                return View(visits);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
