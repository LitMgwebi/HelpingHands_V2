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

        public async Task<IActionResult> Index()
        {
            try
            {
                var wounds = await _wound.GetWounds();

                if (wounds == null)
                {
                    return NotFound();
                }
                ViewBag.Wounds = wounds;
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

                var wound = await _wound.GetWound(id);

                if (wound == null)
                    return NotFound();

                ViewBag.Wound = wound;
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View();
                }
                await _wound.AddWound(wound);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));

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

                var wound = await _wound.GetWound(id);

                if (wound == null)
                    return NotFound();

                ViewBag.Wound = wound;
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
        public async Task<IActionResult> Edit([Bind("WoundId, WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Wound = wound;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View();
                }
                await _wound.UpdateWound(wound);
                return RedirectToAction(nameof(Details), new {id = wound.WoundId});
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
        public async Task<IActionResult> Delete([Bind("WoundId")] int WoundId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Details), new { id = WoundId });
                }
                await _wound.DeleteWound(WoundId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new {id = WoundId});
            }
        }
    }
}
