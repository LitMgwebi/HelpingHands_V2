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
                    ViewBag.Message = "Not all the information was entered, Please look below for what's missing.";
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
                    ViewBag.Message = "Model state not valid";
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
                await _city.DeleteCity(CityId);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
