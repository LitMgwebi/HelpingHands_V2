using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisit _visit;

        public VisitController(IVisit visit)
        {
            _visit = visit;
        }

        public IActionResult Index()
        {
            try
            {
                var visits = _visit.GetVisits();

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

        [HttpGet]
        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var visit = _visit.GetVisit(id);

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
        public IActionResult Create([Bind("ContractId, VisitDate, ApproxTime, Arrival, Departure, WoundCondition, Note, Active")] Visit visit)
        {
            try
            {
                _visit.AddVisit(visit);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
                //ViewBag.Message = "Operation unsuccessful";
                //return View();
            }
        }
    }
}
