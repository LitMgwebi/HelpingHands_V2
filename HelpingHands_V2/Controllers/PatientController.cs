using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles ="P")]
    public class PatientController : Controller
    {
        private readonly IReport _report;
        public PatientController(IReport report)
        {
            _report = report;
        }

        public IActionResult Dashboard(int id)
        {
            var patientContracts = _report.PatientContract(id);
            var patientContract = patientContracts.First();
            IEnumerable<dynamic>? contractVisits = _report.ContractVisits(patientContract.ContractId);

            ViewBag.ContractId = patientContract.ContractId;
            ViewBag.WoundName = patientContract.WoundName;
            ViewBag.Firstname = patientContract.Firstname;
            ViewBag.Lastname = patientContract.Lastname;

            //return new JsonResult(new { patientContract = patientContract });
            ViewBag.ContractVisits = contractVisits.Reverse();
            return View();
        }
        // GET: PatientController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PatientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
