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
                return View(ps);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
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
                return View(ps);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
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

                return View(ps);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Create(int id)
        {
            try
            {
                var suburbs = await _suburb.GetSuburbs();

                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewData["NurseId"] = id;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseId, SuburbId, Active")] PrefferedSuburb prefferedSuburb)
        {
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                ModelState.Remove("Nurse");
                ModelState.Remove("Suburb");
                if (!ModelState.IsValid)
                {
                    ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewData["NurseId"] = prefferedSuburb.NurseId;
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(prefferedSuburb);
                }
                await _ps.AddPrefferedSuburb(prefferedSuburb);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
            }
            catch (Exception ex)
            {
                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewData["NurseId"] = prefferedSuburb.NurseId;
                ViewBag.Message = ex.Message;
                return View(prefferedSuburb);
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
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Details), new { nurseId = prefferedSuburb.NurseId, suburbId = prefferedSuburb.SuburbId });
                }
                await _ps.DeletePrefferedSuburb(prefferedSuburb);
                return RedirectToAction(nameof(IndexForNurse), new { id = prefferedSuburb.NurseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { nurseId = prefferedSuburb.NurseId, suburbId = prefferedSuburb.SuburbId });
            }
        }
    }
}
