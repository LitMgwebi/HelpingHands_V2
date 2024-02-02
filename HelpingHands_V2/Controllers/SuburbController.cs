using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class SuburbController : Controller
    {
        private readonly ISuburb _suburb;
        private readonly ICity _city;

        public SuburbController(ISuburb suburb, ICity city)
        {
            _suburb = suburb;
            _city = city;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                SuburbsViewModel suburbsViewModel;
                IEnumerable<Suburb> suburbs;

                suburbs = await _suburb.GetSuburbs();

                if (suburbs == null)
                {
                    throw new NullReferenceException("Could not retrieve suburbs from the database");
                }

                suburbsViewModel = new SuburbsViewModel
                {
                    Suburbs = suburbs,
                };

                if (suburbsViewModel == null)
                { 
                    return NotFound();
                }

                var cities = await _city.GetCities();
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");

                return View(suburbsViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("SuburbId")]int? SuburbId)
        {
            try
            {
                if (SuburbId == null)
                    return NotFound();

                SuburbsViewModel suburbsViewModel = await CreateModel(SuburbId);
                
                if (suburbsViewModel == null)
                    return NotFound();

                var cities = await _city.GetCities();
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                return View(suburbsViewModel);
            } catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SuburbName, PostalCode, CityId, Active")] Suburb suburb)
        {
            var cities = await _city.GetCities();
            SuburbsViewModel suburbsViewModel = await CreateModel(suburb.SuburbId);
            try
            {
                ModelState.Remove("City");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                    return View(nameof(Index), suburbsViewModel);
                }
                await _suburb.AddSuburb(suburb);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                return View(nameof(Index), suburbsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SuburbId, SuburbName, PostalCode, CityId, Active")] Suburb suburb)
        {
            var cities = await _city.GetCities();
            SuburbsViewModel suburbsViewModel = await CreateModel(suburb.SuburbId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                    return View(nameof(Index), suburbsViewModel);
                }
                await _suburb.UpdateSuburb(suburb);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                return View(nameof(Index), suburbsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("SuburbId")]int SuburbId)
        {
            var cities = await _city.GetCities();
            SuburbsViewModel suburbsViewModel = await CreateModel(SuburbId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                    return View(nameof(Index), suburbsViewModel);
                }
                await _suburb.DeleteSuburb(SuburbId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                return View(nameof(Index), suburbsViewModel);
            }
        }

        public async Task<SuburbsViewModel> CreateModel(int? id = null)
        {
            SuburbsViewModel suburbsViewModel;
            Suburb suburb;
            IEnumerable<Suburb> suburbs;

            suburbs = await _suburb.GetSuburbs();
            suburb = await _suburb.GetSuburb(id);

            if (suburbs == null)
            {
                throw new NullReferenceException("Could not retrieve suburbs from the database"); 
            }

            suburbsViewModel = new SuburbsViewModel
            {
                Suburbs = suburbs,
                Suburb = suburb
            };

            return suburbsViewModel;
        }
    }
}
