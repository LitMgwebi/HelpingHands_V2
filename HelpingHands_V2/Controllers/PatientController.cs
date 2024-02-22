using CloudinaryDotNet.Actions;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    //[Authorize(Roles = "N, A, O")]
    public class PatientController : Controller
    {
        private readonly IPatient _patient;
        private readonly IReport _report;
        private readonly IEndUser _user;
        private readonly ISuburb _suburb;
        public List<SelectListItem> genders = new List<SelectListItem>
        {
            new SelectListItem("Male", "Male"),
            new SelectListItem("Female", "Female"),
            new SelectListItem("Non-Binary", "Non-Binary"),
            new SelectListItem("Gender-fluid", "Gender-fluid"),
            new SelectListItem("Other", "Other"),
        };

        public PatientController(IReport report, IPatient patient, IEndUser user, ISuburb suburb)
        {
            _report = report;
            _patient = patient;
            _user = user;
            _suburb = suburb;
        }

        public async Task<IActionResult> Dashboard(int id)
        {
            List<dynamic> nextVisit = new List<dynamic> { };
            List<dynamic>? contractVisits = new List<dynamic> { };
            try
            {
                //DateTime currentDate = DateTime.Now;
                //var patientContracts = await _report.PatientContract(id);
                //if (patientContracts.Count > 0)
                //{
                //    var patientContract = patientContracts.FirstOrDefault();
                //    contractVisits = await _report.ContractVisits(patientContract!.ContractId);
                //    if(contractVisits.Count > 0)
                //    {
                //        var latestVisit = contractVisits.LastOrDefault();
                //        var result = DateTime.Compare(latestVisit!.VisitDate, currentDate);

                //        if (result > 0)
                //        {
                //            nextVisit.Add(latestVisit);
                //            contractVisits.RemoveAt(1);
                //        }
                //    }
                //    ViewBag.PatientContract = patientContract;
                //}
                //ViewBag.ContractVisits = contractVisits;
                //ViewBag.NextVisit = nextVisit;
                return View();
            }
            catch (Exception ex)
            {
                //ViewBag.ContractVisits = contractVisits;
                //ViewBag.NextVisit = nextVisit;
                //ViewBag.Message = ex.Message + nextVisit + contractVisits;
                return View();
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var patients = await _patient.GetPatients();

                if (patients == null)
                {
                    return NotFound();
                }
                return View(patients);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var patient = await _patient.GetPatient(id);

                if (patient == null)
                    return NotFound();
                return View(patient);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Create(EndUser user)
        {
            try
            {
                var suburbs = await _suburb.GetSuburbs();
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");

                Patient patient = new Patient();
                patient.EndUser = user;
                patient.PatientId = user.UserId;
                return View(patient);
            } catch (Exception ex) {

                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")]Patient patient, [Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                ModelState.Remove("PatientNavigation");   
                ModelState.Remove("Suburb");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                if (!ModelState.IsValid)
                {
                    var suburbs = await _suburb.GetSuburbs();
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    patient.EndUser = user;
                    return View(patient);
                }
                await _patient.AddPateint(patient);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Dashboard), new {id = patient.PatientId});
            }
            catch (Exception ex)
            {
                patient.EndUser = user;
                ViewBag.Message = ex.Message;
                return View(patient);
            }
        }


        [Authorize(Roles ="P")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var patient = await _patient.GetPatient(id);
                var user = await _user.GetUserById(id);
                var suburbs = await _suburb.GetSuburbs();

                if (patient == null || suburbs == null || user == null)
                    return NotFound();

                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewData["Genders"] = genders;
                return View(patient);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")] Patient patient, IFormFile? file, [Bind("UserId, Username, Firstname, Lastname, DateOfBirth, Email, Password, Gender, ContactNumber, Idnumber, UserType, ApplicationType, ProfilePicture, ProfilePictureName, Active")] EndUser user)
        {
            try
            {
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("Password");
                if (!ModelState.IsValid)
                {
                    var suburbs = await _suburb.GetSuburbs();
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewData["Genders"] = genders;
                    return View(patient);
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

                await _user.UpdateUser(user);
                await _patient.UpdatePatient(patient);
                return RedirectToAction(nameof(Profile), new { id = patient.PatientId });
            }
            catch (Exception ex)
            {
                ViewData["Genders"] = genders;
                ViewBag.Message = ex.Message;
                return View(patient);
            }
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("PatientId")] int PatientId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Profile), new { id = PatientId });
                }
                await _patient.DeletePatient(PatientId);
                await _user.DeleteUser(PatientId);
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Profile), new { id = PatientId });
            }
        }
    }
}
