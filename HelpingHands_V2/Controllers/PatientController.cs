using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                DateTime currentDate = DateTime.Now;
                var patientContracts = await _report.PatientContract(id);
                if (patientContracts.Count > 0)
                {
                    var patientContract = patientContracts.FirstOrDefault();

                    contractVisits = await _report.ContractVisits(patientContract!.ContractId);

                    if(contractVisits.Count > 0)
                    {
                        var latestVisit = contractVisits.LastOrDefault();
                        var result = DateTime.Compare(latestVisit!.VisitDate, currentDate);

                        if (result > 0)
                        {
                            nextVisit.Add(latestVisit);
                            contractVisits.RemoveAt(1);
                        }
                    }
                    ViewBag.PatientContract = patientContract;
                }
                ViewBag.ContractVisits = contractVisits;
                ViewBag.NextVisit = nextVisit;
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { nextVisit, contractVisits, ex.Message });
                ViewBag.ContractVisits = contractVisits;
                ViewBag.NextVisit = nextVisit;
                ViewBag.Message = ex.Message + nextVisit + contractVisits;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.Patients = patients;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                var user = await _user.GetUserById(id);

                if (patient == null)
                    return NotFound();

                ViewBag.Patient = patient;
                ViewBag.User = user;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var suburbs = await _suburb.GetSuburbs();
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewData["PatientId"] = id;
                return View();
            } catch (Exception ex) {

                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")]Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var suburbs = await _suburb.GetSuburbs();
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewData["PatientId"] = patient.PatientId;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _patient.AddPateint(patient);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Dashboard), new {id = patient.PatientId});
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }


        // GET: PatientController/Edit/5
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

                if (patient == null || user == null || suburbs == null)
                    return NotFound();

                ViewBag.Patient = patient;
                ViewBag.User = user;
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")] Patient patient)
        {
            try
            {
                ModelState.Remove("PatientNavigation");
                ModelState.Remove("Suburb");
                if (!ModelState.IsValid)
                {
                    var user = await _user.GetUserById(patient.PatientId);
                    var suburbs = await _suburb.GetSuburbs();
                    ViewBag.User = user;
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _patient.UpdatePatient(patient);
                return RedirectToAction(nameof(Profile), new { id = patient.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
