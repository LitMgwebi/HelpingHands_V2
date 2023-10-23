using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class PatientConditionController : Controller
    {
        private readonly IPatientCondition _pc;
        private readonly IPatient _patient;
        private readonly ICondition _condition;

        public PatientConditionController (IPatientCondition pc, ICondition condition, IPatient patient)
        {
            _pc = pc;
            _condition = condition;
            _patient = patient;
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

        public IActionResult IndexForPatient(int? id)
        {
            try
            {
                var pc = _pc.GetPatientConditionsByPatient(id);

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
        public IActionResult Details(int? patientId, int? conditionId)
        {
            try
            {
                if (patientId == null && conditionId == null)
                {
                    return NotFound();
                }

                var pc = _pc.GetOnePatientCondition(patientId, conditionId);

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

        public IActionResult Create(int id)
        {
            try
            {
                var conditions = _condition.GetConditions();

                ViewData["ConditionId"] = new SelectList(conditions, "ConditionId", "ConditionName");
                ViewData["PatientId"] = id;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            try
            {
                _pc.AddPatientCondition(patientCondition);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
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
