using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
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

                WoundsViewModel woundsViewModel = new WoundsViewModel
                {
                    Wounds = wounds
                };

                return View(woundsViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("WoundId")]int? WoundId)
        {
            try
            {
                if (WoundId == null)
                {
                    return NotFound();
                }

                var wound = await _wound.GetWound(WoundId);
                var wounds = await _wound.GetWounds();

                if (wound == null || wounds == null)
                    return NotFound();

                WoundsViewModel woundsViewModel = new WoundsViewModel { 
                    Wounds = wounds,
                    Wound = wound
                };

                return View(woundsViewModel);
            }
            catch (Exception ex)
            {
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
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("WoundId, WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return RedirectToAction(nameof(Index));
                }
                await _wound.UpdateWound(wound);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View(wound);
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
                    return RedirectToAction(nameof(Index));
                }
                await _wound.DeleteWound(WoundId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
