using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using HelpingHands_V2.ViewModels;
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
                var cities = await _city.GetCities();

                if (cities == null)
                {
                    return NotFound();
                }
                CitiesAndCity citiesAndCity = new CitiesAndCity
                {
                    Cities = cities
                };
                return View(citiesAndCity);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("CityId")]int? CityId)
        {
            try
            {
                if (CityId == null)
                {
                    return NotFound();
                }

                var city = await _city.GetCity(CityId);
                var cities = await _city.GetCities();

                if (city == null || cities == null)
                    return NotFound();

                CitiesAndCity citiesAndCity = new CitiesAndCity
                {
                    Cities = cities,
                    City = city
                };

                return View(citiesAndCity);
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
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View();
                }
                await _city.AddCity(city);
                ViewBag.Message = "Record Added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
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
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(city);
                }
                await _city.UpdateCity(city);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(city);
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
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Index));
                }
                await _city.DeleteCity(CityId);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
