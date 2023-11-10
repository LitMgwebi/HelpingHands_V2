using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class CityController : Controller
    {
        private readonly ICity _city;

        public CityController(ICity city)
        {
            _city = city;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var city = await _city.GetCities();

                if (city == null)
                {
                    return NotFound();
                }
                ViewBag.Cities = city;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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

                var city = await _city.GetCity(id);

                if (city == null)
                    return NotFound();

                ViewBag.City = city;
                return View();
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
        public async Task<IActionResult> Create([Bind("CityName, CityAbbreviation, Active")] City city)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                await _city.AddCity(city);
                ViewBag.Message = "Record Added successfully.";
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

                var city = await _city.GetCity(id);

                if (city == null)
                    return NotFound();

                ViewBag.City = city;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CityId, CityName, CityAbbreviation, Active")] City city)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.City = city;
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                await _city.UpdateCity(city);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("CityId")]int CityId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return RedirectToAction(nameof(Details), new { id = CityId });
                }
                await _city.DeleteCity(CityId);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { id = CityId });
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
