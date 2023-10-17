using HelpingHands_V2.Interfaces;
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
    }
}
