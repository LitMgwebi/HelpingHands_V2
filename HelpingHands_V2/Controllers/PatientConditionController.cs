using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var pc = await _pc.GetPatientConditions();

                if (pc == null)
                {
                    return NotFound();
                }
                return View(pc);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> IndexForPatient(int? id)
        {
            try
            {
                var pc = await _pc.GetPatientConditionsByPatient(id);

                if (pc == null)
                {
                    return NotFound();
                }

                var conditions = await _condition.GetConditions();

                PatientConditionViewModel pcViewModel = new PatientConditionViewModel
                {
                    PatientId = id,
                    Conditions = pc
                };

                ViewData["Conditions"] = new SelectList(conditions, "ConditionId", "ConditionName");
                return View(pcViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            var conditions = await _condition.GetConditions();
            try
            {
                ModelState.Remove("Condition");
                ModelState.Remove("Patient");
                if (!ModelState.IsValid)
                {
                    ViewData["Conditions"] = new SelectList(conditions, "ConditionId", "ConditionName");
                    ViewData["PatientId"] = patientCondition.PatientId;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
                }
                await _pc.AddPatientCondition(patientCondition);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
            catch (Exception ex)
            {
                ViewData["Conditions"] = new SelectList(conditions, "ConditionId", "ConditionName");
                ViewData["PatientId"] = patientCondition.PatientId;
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            try
            {
                ModelState.Remove("Condition");
                ModelState.Remove("Patient");
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
                }
                await _pc.DeletePatientCondition(patientCondition);
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
        }
    }
}
