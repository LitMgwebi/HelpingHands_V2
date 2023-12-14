using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        private readonly INurse _nurse;
        private readonly IEndUser _user;
        private readonly ISuburb _suburb;
        private readonly ICity _city;

        public AdminController(INurse nurse, IEndUser user, ISuburb suburb, ICity city)
        {
            _nurse = nurse;
            _user = user;
            _suburb = suburb;
            _city = city;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var nurses = await _nurse.GetNurses();
                var managers = await _user.GetManagers();
                var cities = await _city.GetCities();
                var suburbs = await _suburb.GetSuburbs();

                if(nurses == null || managers == null || cities == null || suburbs == null)
                {
                    return NotFound();
                }

                ViewBag.Nurses = nurses;
                ViewBag.Managers = managers;
                ViewBag.Cities = cities;
                ViewBag.Suburbs = suburbs;

                return View();
            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
