using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManager _manager;

        public ManagerController(IManager manager) => _manager = manager;

        public IActionResult Dashboard(int id)
        {
            try
            {
                var availableNurses = _manager.AvailableNurses(id);
                var careVisits = _manager.CareVisits(new DateTime(2023, 4, 01), new DateTime(2023, 8, 01));
                var contractStatus = _manager.ContractStatus("N");
                var contractVisits = _manager.ContractVisits(4);
                var patientContract = _manager.PatientContract(id);
                
                if(availableNurses == null || careVisits == null || contractStatus == null || contractVisits == null || patientContract == null)
                {
                    return NotFound();
                }

                ViewBag.AvailableNurses = availableNurses;
                ViewBag.CareVisits = careVisits;
                ViewBag.ContractStatus = contractStatus;
                ViewBag.ContractVisits = contractVisits;
                ViewBag.PatientContract = patientContract;

                return View();
            } catch(Exception ex)
            {
                return new JsonResult(new { error = ex });
            }
        }

        // GET: ManagerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManagerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagerController/Create
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

        // GET: ManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManagerController/Edit/5
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

        // GET: ManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManagerController/Delete/5
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
