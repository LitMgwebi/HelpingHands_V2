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

        [Authorize(Roles ="N, O, A")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var visits = await _visit.GetVisits();

                if (visits == null)
                {
                    return NotFound();
                }
                ViewBag.Visits = visits;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        public IActionResult IndexForUser(int id)
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };
                List<dynamic> contractVisits = new List<dynamic> { };
                List<dynamic> nextVisit = new List<dynamic> { };

                contracts = _report.PatientContract(id);
                if (contracts.Count > 0)
                {
                    foreach (var contract in contracts)
                    {
                        contractVisits = _report.ContractVisits(contract.ContractId);

                        if (contractVisits.Count > 0)
                        {
                            foreach (var visit in contractVisits)
                            {
                                nextVisit.Add(visit);
                            }
                        }
                    }
                }
                if (nextVisit.Count > 0)
                {
                    for (int i = 0; i < nextVisit.Count - 1; i++)
                        for (int j = 0; j < nextVisit.Count - i - 1; j++)
                            if (nextVisit[j].VisitDate > nextVisit[j + 1].VisitDate)
                            {
                                var tempVar = nextVisit[j];
                                nextVisit[j] = nextVisit[j + 1];
                                nextVisit[j + 1] = tempVar;
                            }
                }

                //if (contracts == null)
                //{
                //    return NotFound();
                //}
                ViewBag.Visits = nextVisit;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message, ex.Source });
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

                ViewBag.Visit = visit;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "N")]
        public IActionResult Create(int id)
        {
            try
            {
                ViewBag.ContractId = id;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId, VisitDate, ApproxTime, Arrival, Departure, WoundCondition, Note, Active")] Visit visit)
        {
            try
            {
                var userId = HttpContext.User.FindFirst("UserId")!.Value;
                await _visit.AddVisit(visit);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Dashboard", "Nurse", new {id = userId});
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
                //ViewBag.Message = "Operation unsuccessful";
                //return View();
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

                ViewBag.Visit = visit;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("VisitId, ContractId, VisitDate, ApproxTime, Arrival, Departure, WoundCondition, Note, Active")] Visit visit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _visit.UpdateVisit(visit);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("VisitId")] int VisitId)
        {
            try
            {
                await _visit.DeleteVisit(VisitId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
