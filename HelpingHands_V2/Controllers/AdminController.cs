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
        private readonly Grp0444HelpingHandsContext _context;
        private readonly INurse _nurse;
        private readonly IManager _manager;
        private readonly ISuburb _suburb;
        private readonly ICity _city;

        public AdminController(Grp0444HelpingHandsContext context, INurse nurse, IManager manager, ISuburb suburb, ICity city)
        {
            _context = context;
            _nurse = nurse;
            _manager = manager;
            _suburb = suburb;
            _city = city;
        }
        public ActionResult Dashboard()
        {
            try
            {
                var nurses = _nurse.GetNurses();
                var managers = _manager.GetManagers();
                var cities = _city.GetCities();
                var suburbs = _suburb.GetSuburbs();

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
                return new JsonResult(new { error = ex.Message });
            }
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
