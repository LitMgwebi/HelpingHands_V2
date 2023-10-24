using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    //[Authorize(Roles = "N, A, O")]
    public class PatientController : Controller
    {
        private readonly IPatient _patient;
        private readonly IReport _report;
        private readonly IEndUser _user;
        public PatientController(IReport report, IPatient patient, IEndUser user)
        {
            _report = report;
            _patient = patient;
            _user = user;
        }

        public IActionResult Dashboard(int id)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var patientContracts = _report.PatientContract(id);
                var patientContract = patientContracts.First();
                List<dynamic> nextVisit = new List<dynamic>();
                List<dynamic>? contractVisits = _report.ContractVisits(patientContract.ContractId);

                var latestVisit = contractVisits.Last();

                var result = DateTime.Compare(latestVisit.VisitDate, currentDate);

                if (result > 0)
                {
                    nextVisit.Add(latestVisit);
                    contractVisits.RemoveAt(1);
                }
                ViewBag.PatientContract = patientContract;

                //return new JsonResult(new { patientContract = patientContract });
                ViewBag.ContractVisits = contractVisits.AsEnumerable().Reverse();
                ViewBag.NextVisit = nextVisit;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        // GET: PatientController
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

        // GET: PatientController/Details/5
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

        // GET: PatientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PatientController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PatientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
