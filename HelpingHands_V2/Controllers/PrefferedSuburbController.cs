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

        public async Task<IActionResult> Index()
        {
            try
            {
                var ps = await _ps.GetPrefferedSuburbs();

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

        public async Task<IActionResult> IndexForNurse(int? id)
        {
            try
            {
                var ps = await _ps.GetPrefferedSuburbsByNurse(id);

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
        public async Task<IActionResult> Details(int? nurseId, int? suburbId)
        {
            try
            {
                if (nurseId == null && suburbId == null)
                {
                    return NotFound();
                }

                var ps = await _ps.GetPrefferedSuburb(nurseId, suburbId);

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

        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var suburbs = await _suburb.GetSuburbs();

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
        public async Task<IActionResult> Create([Bind("NurseId, SuburbId, Active")] PrefferedSuburb prefferedSuburb)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _ps.AddPrefferedSuburb(prefferedSuburb);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("NurseId, SuburbId, Active")] PrefferedSuburb prefferedSuburb)
        {
            try
            {
                await _ps.DeletePrefferedSuburb(prefferedSuburb);
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
