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
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                    var suburbs = await _suburb.GetSuburbs();
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewData["NurseId"] = prefferedSuburb.NurseId;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                await _ps.AddPrefferedSuburb(prefferedSuburb);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
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
        public async Task<IActionResult> Delete([Bind("NurseId, SuburbId, Active")] PrefferedSuburb prefferedSuburb)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return RedirectToAction(nameof(Details), new { nurseId = prefferedSuburb.NurseId, suburbId = prefferedSuburb.SuburbId });
                }
                await _ps.DeletePrefferedSuburb(prefferedSuburb);
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { nurseId = prefferedSuburb.NurseId, suburbId = prefferedSuburb.SuburbId });
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
