using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class CityController : Controller
    {
        private readonly ICity _city;

        public CityController(ICity city)
        {
            _city = city;
        }
        public IActionResult Index()
        {
            try
            {
                var city = _city.GetCities();

                if (city == null)
                {
                    return NotFound();
                }
                ViewBag.Cities = city;
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
                if(id == null)
                {
                    return NotFound();
                }

                var city = _city.GetCity(id);

                if(city == null)
                    return NotFound();

                ViewBag.City = city;
                return View();
            } catch (Exception ex)
            {
                return new JsonResult(new {error = ex.Message});
            }
        }

        public IActionResult Create()
        {
            try
            {
                return View();
            } catch(Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(City city)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _city.AddCity(city);
                    ViewBag.Message = "Record Added successfully;";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Message = "Operation unsuccessful";
                return new JsonResult(new { error = "Operation Unsuccessful" });
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
