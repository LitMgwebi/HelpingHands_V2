using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class WoundController : Controller
    {
        private readonly IWound _wound;

        public WoundController(IWound wound)
        {
            _wound = wound;
        }

        public IActionResult Index()
        {
            try
            {
                var wounds = _wound.GetWounds();

                if (wounds == null)
                {
                    return NotFound();
                }
                ViewBag.Wounds = wounds;
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

                var wound = _wound.GetWound(id);

                if (wound == null)
                    return NotFound();

                ViewBag.Wound = wound;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _wound.AddWound(wound);
                    ViewBag.Message = "Record Added successfully;";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Message = "Operation unsuccessful";
                return View();
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
