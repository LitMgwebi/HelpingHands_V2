using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
