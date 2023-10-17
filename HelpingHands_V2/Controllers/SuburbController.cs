using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class SuburbController : Controller
    {
        private readonly ISuburb _suburb;

        public SuburbController(ISuburb suburb)
        {
            _suburb = suburb;
        }

        public IActionResult Index()
        {
            try
            {
                var suburbs = _suburb.GetSuburbs();

                if (suburbs == null)
                {
                    return NotFound();
                }
                ViewBag.Suburbs = suburbs;
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

                var suburb = _suburb.GetSuburb(id);

                if (suburb == null)
                    return NotFound();

                ViewBag.Suburb = suburb;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
