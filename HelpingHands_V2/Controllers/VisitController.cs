using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisit _visit;
        private readonly IReport _report;

        public VisitController(IVisit visit, IReport report)
        {
            _visit = visit;
            _report = report;
        }

        public async Task<IActionResult> Index(int? id, string? command)
        {
            try
            {
                IEnumerable<Visit> visits = new List<Visit>();
                DateTime currentDate = DateTime.Now;

                if (command == "nurse")
                    visits = await _report.ContractVisits(id);
                else if (command == "patient")
                {
                    visits = await _report.PatientVisits(id);
                }
                else
                    visits = await _visit.GetVisits();

                if (visits == null)
                {
                    return NotFound();
                }
                return View(visits);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var visit = await _visit.GetVisit(id);

                if (visit == null)
                    return NotFound();

                //ViewBag.Visit = visit;
                return View(visit);
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
        public async Task<IActionResult> Create([Bind("ContractId, VisitDate, ApproxTime, Arrival, Departure, WoundCondition, Note, Active")] Visit visit)
        {
            try
            {
                ModelState.Remove("Contract");
                if (!ModelState.IsValid)
                {
                    ViewBag.ContractId = visit.ContractId;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return RedirectToAction("Details", "Contract", new { id = visit.ContractId });
                }
                var userId = HttpContext.User.FindFirst("UserId")!.Value;
                await _visit.AddVisit(visit);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Details", "Contract", new { id = visit.ContractId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("Details", "Contract", new { id = visit.ContractId });
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

                var visit = await _visit.GetVisit(id);

                if (visit == null)
                    return NotFound();

                //ViewBag.Visit = visit;
                return View(visit);
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
        public async Task<IActionResult> Edit([Bind("VisitId, ContractId, VisitDate, ApproxTime, Arrival, Departure, WoundCondition, Note, Active")] Visit visit)
        {
            try
            {
                ModelState.Remove("Contract");
                if (!ModelState.IsValid)
                {
                    //ViewBag.Visit = visit;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    ViewBag.ContractId = visit.ContractId;
                    return View(visit);
                }
                await _visit.UpdateVisit(visit);
                if (HttpContext.User.IsInRole("N"))
                {
                    return RedirectToAction("Visits", "Nurse", new { id = HttpContext.User.FindFirst("UserId")!.Value, command = "upcoming" });
                }
                else
                {
                    return RedirectToAction(nameof(Details), new {id = visit.VisitId});
                }
            }
            catch (Exception ex)
            {
                ViewBag.ContractId = visit.ContractId;
                ViewBag.Message = ex.Message;
                return View(visit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("VisitId")] int VisitId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Details), new { id = VisitId });
                }
                await _visit.DeleteVisit(VisitId); 
                
                if (HttpContext.User.IsInRole("N"))
                {
                    return RedirectToAction("Visits", "Nurse", new { id = HttpContext.User.FindFirst("UserId")!.Value, command = "upcoming" });
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { id = VisitId });
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
