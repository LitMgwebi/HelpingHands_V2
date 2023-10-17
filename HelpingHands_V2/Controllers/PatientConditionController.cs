using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class PatientConditionController : Controller
    {
        private readonly IPatientCondition _pc;

        public PatientConditionController (IPatientCondition pc)
        {
            _pc = pc;
        }
        public IActionResult Index()
        {
            try
            {
                var pc = _pc.GetPatientConditions();

                if (pc == null)
                {
                    return NotFound();
                }
                ViewBag.PatientConditions = pc;
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
                if (id == null)
                {
                    return NotFound();
                }

                var pc = _pc.GetPatientConditionByCondition(id);

                if (pc == null)
                    return NotFound();

                ViewBag.PatientCondition = pc;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
