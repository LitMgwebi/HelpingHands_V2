using CloudinaryDotNet.Actions;
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
                WoundsViewModel woundsViewModel = await CreateModel();

                if (woundsViewModel == null)
                    return NotFound();

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

                WoundsViewModel woundsViewModel = await CreateModel(WoundId);

                if(woundsViewModel == null) 
                    return NotFound();

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
        public async Task<IActionResult> Create([Bind("WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            WoundsViewModel woundsViewModel = await CreateModel(wound.WoundId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View(nameof(Index), woundsViewModel);
                }
                await _wound.AddWound(wound);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), woundsViewModel);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("WoundId, WoundName, Grade, WoundDescription, Active")] Wound wound)
        {
            WoundsViewModel woundsViewModel = await CreateModel(wound.WoundId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View(nameof(Index), woundsViewModel);
                }
                await _wound.UpdateWound(wound);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), woundsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("WoundId")] int WoundId)
        {
            WoundsViewModel woundsViewModel = await CreateModel(WoundId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return View(nameof(Index), woundsViewModel);
                }
                await _wound.DeleteWound(WoundId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), woundsViewModel);
            }
        }

        public async Task<WoundsViewModel> CreateModel(int? id = null)
        {
            var wound = await _wound.GetWound(id);
            var wounds = await _wound.GetWounds();

            if (wounds == null)
                throw new NullReferenceException("Could not retrieve cities from the database");

            WoundsViewModel woundsViewModel = new WoundsViewModel
            {
                Wounds = wounds,
                Wound = wound
            };

            return woundsViewModel;
        }
    }
}
