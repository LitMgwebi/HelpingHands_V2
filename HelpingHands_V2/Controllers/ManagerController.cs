using CloudinaryDotNet.Actions;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Contracts;
using System.Net.Mail;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "O, A")]
    public class ManagerController : Controller
    {
        private readonly IReport _report;
        private readonly IVisit _visit;
        private readonly INurse _nurse;
        private readonly IPatient _patient;
        private readonly IContract _contract;
        private readonly IEndUser _user;
        private readonly IEmailSender _email;

        public ManagerController(IReport report, IVisit visit, INurse nurse, IContract contract, IPatient patient, IEndUser user, IEmailSender email)
        {
            _report = report;
            _visit = visit;
            _nurse = nurse;
            _contract = contract;
            _patient = patient;
            _user = user;
            _email = email;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                List<CareContract> newContracts = new List<CareContract> { };
                List<CareContract> assignedContracts = new List<CareContract> { };
                List<dynamic> visits = new List<dynamic> { };

                newContracts = await _report.ContractStatus("N");
                assignedContracts = await _report.ContractStatus("A");

                if (newContracts == null || assignedContracts == null)
                {
                    return NotFound();
                }

                ViewBag.NewContracts = newContracts;
                ViewBag.AssignedContracts = assignedContracts;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> NewContracts()
        {
            try
            {
                List<CareContract> contracts = new List<CareContract> { };

                contracts = await _report.ContractStatus("N");
                return View(contracts);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> VisitRange()
        {
            try
            {
                IEnumerable<Nurse> nurses = await _nurse.GetNurses();
                IEnumerable<EndUser> users = await _nurse.GetUsersByIDs(nurses);

                ViewData["Nurses"] = new SelectList(users, "UserId", "FullName");
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
            IEnumerable<Nurse> nurses = await _nurse.GetNurses();
            IEnumerable<EndUser> users = await _nurse.GetUsersByIDs(nurses);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Nurses"] = new SelectList(users, "UserId", "FullName");
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                visitRange = await _report.NurseVisitRange(NurseId, StartDate, EndDate);
                return View(visitRange);
            }
            catch (Exception ex)
            {
                ViewData["Nurses"] = new SelectList(users, "UserId", "FullName");
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> ContractRange()
        {
            try
            {

                IEnumerable<Patient> patients = await _patient.GetPatients();
                IEnumerable<EndUser> users = await _patient.GetUsersByIDs(patients);

                ViewData["Patients"] = new SelectList(users, "UserId", "FullName");
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
        public async Task<IActionResult> ContractRange(int PatientId, DateTime StartDate, DateTime EndDate)
        {
            IEnumerable<CareContract> contractRange;
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                contractRange = await _report.CareVisits(PatientId, StartDate, EndDate);
                return View(contractRange);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> ContractDetails(int? id)
        {
            List<dynamic> nurses = new List<dynamic> { };
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contract = await _contract.GetContract(id);

                if (contract == null)
                    return NotFound();

                ViewBag.Contract = contract;
                ViewBag.Nurses = nurses;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Contract = new { };
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContractDetails(int ContractId)
        {
            List<dynamic> nurses = new List<dynamic> { };
            var contract = await _contract.GetContract(ContractId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Contract = contract;
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                nurses = await _report.AvailableNurses(ContractId);
                ViewBag.Nurses = nurses;
                ViewBag.Contract = contract;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Nurses = nurses;
                ViewBag.Contract = contract;
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Can't send model via input tag from Nurse Profile to this action method
        public async Task<IActionResult> ApproveNurse([Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, ConfirmPassword, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction("Profile", "Nurse", new { id = user.UserId });
                }
                user.UserType = "N";
                await _user.UpdateUser(user);

                Message emailMessage = new Message(new string[] { user.Email! }, user.FullName, user.Username, "patient_registering");
                _email.SendEmail(emailMessage);
                return RedirectToAction("Index", "Nurse", new { command = "waiting" });
            } catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("Profile", "Nurse", new { id = user.UserId });
            }
        }
    }
}
