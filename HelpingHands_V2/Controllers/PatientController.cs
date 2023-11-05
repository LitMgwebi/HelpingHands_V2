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

        public IActionResult Dashboard(int id)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var patientContracts = _report.PatientContract(id);
                List<dynamic> nextVisit = new List<dynamic> { };
                List<dynamic>? contractVisits = new List<dynamic> { };
                if (patientContracts.Count > 0)
                {
                    var patientContract = patientContracts.FirstOrDefault();

                    contractVisits = _report.ContractVisits(patientContract!.ContractId);

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
                return new JsonResult(new { error = ex.Message, content = ex.Data, moreContent = ex.Source, innerException = ex.InnerException });
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
                return new JsonResult(new { error = ex.Message });
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
                return new JsonResult(new { error = ex.Message });
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
            } catch (Exception ex) { return new JsonResult(new { error = ex.Message }); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")]Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _patient.AddPateint(patient);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Dashboard), new {id = patient.PatientId});
            }
            catch (Exception ex) { return new JsonResult(new { error = ex.Message }); }
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
                return new JsonResult(new { error = ex.Message });
            }
        }
        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PatientId, AddressLineOne, AddressLineTwo, SuburbId, Icename, Icenumber, AdditionalInfo, Active")] Patient patient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _patient.UpdatePatient(patient);
                return RedirectToAction(nameof(Profile), new { id = patient.PatientId });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("PatientId")] int PatientId)
        {
            try
            {
                await _patient.DeletePatient(PatientId);
                await _user.DeleteUser(PatientId);
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
