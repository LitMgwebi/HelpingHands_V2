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
                CitiesViewModel citiesViewModel = await CreateModel();

                if (citiesViewModel == null)
                {
                    return NotFound();
                } 

                return View(citiesViewModel);
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
                    return NotFound();
               
                CitiesViewModel citiesViewModel = await CreateModel(CityId);

                if (citiesViewModel == null)
                    return NotFound();

                return View(citiesViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("CityName, CityAbbreviation, Active")] City city)
        {
            CitiesViewModel citiesViewModel = await CreateModel(city.CityId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View(nameof(Index), citiesViewModel);
                }
                await _city.AddCity(city);
                ViewBag.Message = "Record Added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), citiesViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CityId, CityName, CityAbbreviation, Active")] City city)
        {
            CitiesViewModel citiesViewModel = await CreateModel(city.CityId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.City = city;
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(nameof(Index), citiesViewModel);
                }
                await _city.UpdateCity(city);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), citiesViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("CityId")]int CityId)
        {
            CitiesViewModel citiesViewModel = await CreateModel(CityId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return View(nameof(Index), citiesViewModel);
                }
                await _city.DeleteCity(CityId);
                return RedirectToAction(nameof(Index));
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), citiesViewModel);
            }
        }

        public async Task<CitiesViewModel> CreateModel(int? id = null)
        {
            CitiesViewModel citiesViewModel;
            City city;
            IEnumerable<City> cities;

            cities = await _city.GetCities();
            city = await _city.GetCity(id);
           
            if (cities == null)
            {
                throw new NullReferenceException("Could not retrieve cities from the database");
            }
            citiesViewModel = new CitiesViewModel
            {
                Cities = cities,
                City = city
            };

            return citiesViewModel;
        }
    }
}
