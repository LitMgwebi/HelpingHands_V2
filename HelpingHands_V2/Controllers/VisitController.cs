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
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        public async Task<IActionResult> IndexForUser(int id, string command)
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };
                List<dynamic> contractVisits = new List<dynamic> { };
                List<dynamic> nextVisit = new List<dynamic> { };

                if (command== "nurse")
                {
                        contractVisits = await _report.ContractVisits(id);

                    ViewBag.Visits = contractVisits;
                    //if (contractVisits.Count > 0)
                    //    {
                    //        foreach (var visit in contractVisits)
                    //        {
                    //            nextVisit.Add(visit);
                    //        }
                    //    }
                } else
                {
                    contracts = await _report.PatientContract(id);
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

                    ViewBag.Visits = nextVisit;
                }

                //if (contracts == null)
                //{
                //    return NotFound();
                //}
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
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

                ViewBag.Visit = visit;
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
                    return new JsonResult(new { visit, errors });
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View();
                }
                var userId = HttpContext.User.FindFirst("UserId")!.Value;
                await _visit.AddVisit(visit);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Dashboard", "Nurse", new {id = userId});
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
                    ViewBag.Visit = visit;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View();
                }
                await _visit.UpdateVisit(visit);
                if (HttpContext.User.IsInRole("N"))
                {
                    return RedirectToAction("Visits", "Nurse", new {id = HttpContext.User.FindFirst("UserId")!.Value, command = "upcoming"});
                } else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Visit = visit;
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
                await _visit.DeleteVisit(VisitId); if (HttpContext.User.IsInRole("N"))
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
                return RedirectToAction(nameof(Details), new {id = VisitId});
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
