using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class PrefferedSuburbController : Controller
    {
        private readonly IPrefferedSuburb _ps;
        private readonly ISuburb _suburb;
        private readonly INurse _nurse;

        public PrefferedSuburbController(IPrefferedSuburb ps, ISuburb suburb, INurse nurse)
        {
            _ps = ps;
            _suburb = suburb;
            _nurse = nurse;
        }
        public IActionResult Index()
        {
            try
            {
                var ps = _ps.GetPrefferedSuburbs();

                if (ps == null)
                {
                    return NotFound();
                }
                ViewBag.PrefferedSuburbs = ps;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult IndexForNurse(int? id)
        {
            try
            {
                var ps = _ps.GetPrefferedSuburbsByNurse(id);

                if (ps == null)
                {
                    return NotFound();
                }
                ViewBag.PrefferedSuburbs = ps;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Details(int? nurseId, int? suburbId)
        {
            try
            {
                if (nurseId == null && suburbId == null)
                {
                    return NotFound();
                }

                var ps = _ps.GetPrefferedSuburb(nurseId, suburbId);

                if (ps == null)
                    return NotFound();

                ViewBag.PrefferedSuburb = ps;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult Create(int id)
        {
            try
            {
                var suburbs  = _suburb.GetSuburbs();

                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewData["NurseId"] = id;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NurseId, SuburbId, Active")] PrefferedSuburb prefferedSuburb)
        {
            try
            {
                _ps.AddPrefferedSuburb(prefferedSuburb);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
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
