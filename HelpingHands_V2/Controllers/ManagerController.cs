using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "O, A")]
    public class ManagerController : Controller
    {
        private readonly IReport _report;
        private readonly IVisit _visit;
        private readonly INurse _nurse;

        public ManagerController(IReport report, IVisit visit, INurse nurse)
        {
            _report = report;
            _visit = visit;
            _nurse = nurse;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                List<dynamic> newContracts = new List<dynamic> { };
                List<dynamic> assignedContracts = new List<dynamic> { };
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
                List<dynamic> contracts = new List<dynamic> { };

                contracts = await _report.ContractStatus("N");

                ViewBag.Contracts = contracts;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> VisitRange()
        {
            List<dynamic> visitRange = new List<dynamic> { };
            try
            {
                var nurses = await _nurse.GetNurses();
                ViewBag.VisitRange = visitRange;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VisitRange(int NurseId, DateTime StartDate, DateTime EndDate)
        {
            List<dynamic> visitRange = new List<dynamic> { };
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    var nurses = await _nurse.GetNurses();
                    ViewBag.VisitRange = visitRange;
                    ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                visitRange = await _report.NurseVisitRange(NurseId, StartDate, EndDate);
                ViewBag.VisitRange = visitRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.VisitRange = visitRange;
                var nurses = await _nurse.GetNurses();
                ViewBag.VisitRange = visitRange;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult ContractRange()
        {
            List<dynamic> contractRange = new List<dynamic> { };
            try
            {
                ViewBag.ContractRange = contractRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ContractRange = contractRange;
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContractRange(DateTime StartDate, DateTime EndDate)
        {
            List<dynamic> contractRange = new List<dynamic> { };
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                contractRange = await _report.CareVisits(StartDate, EndDate);
                ViewBag.ContractRange = contractRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ContractRange = contractRange;
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
