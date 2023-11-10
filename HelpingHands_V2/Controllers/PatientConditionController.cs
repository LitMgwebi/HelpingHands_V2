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

        public async Task<IActionResult> Index()
        {
            try
            {
                var pc = await _pc.GetPatientConditions();

                if (pc == null)
                {
                    return NotFound();
                }
                ViewBag.PatientConditions = pc;
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.PatientConditions = pc;
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
        public async Task<IActionResult> Details(int? patientId, int? conditionId)
        {
            try
            {
                if (patientId == null && conditionId == null)
                {
                    return NotFound();
                }

                var pc = await _pc.GetOnePatientCondition(patientId, conditionId);

                if (pc == null)
                    return NotFound();

                ViewBag.PatientCondition = pc;
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
                var conditions = await _condition.GetConditions();

                ViewData["ConditionId"] = new SelectList(conditions, "ConditionId", "ConditionName");
                ViewData["PatientId"] = id;
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
        public async Task<IActionResult> Create([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var conditions = await _condition.GetConditions();
                    ViewData["ConditionId"] = new SelectList(conditions, "ConditionId", "ConditionName");
                    ViewData["PatientId"] = patientCondition.PatientId;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View();
                }
                await _pc.AddPatientCondition(patientCondition);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Edit(int? patientId, int? conditionId)
        {
            try
            {
                if (patientId == null && conditionId == null)
                {
                    return NotFound();
                }

                var pc = await _pc.GetOnePatientCondition(patientId, conditionId);
                var conditions = await _condition.GetConditions();

                if (pc == null || conditions == null)
                    return NotFound();

                ViewData["ConditionId"] = new SelectList(conditions, "ConditionId", "ConditionName");
                ViewBag.PatientCondition = pc;
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
        public async Task<IActionResult> Edit([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var conditions = await _condition.GetConditions();
                    ViewData["ConditionId"] = new SelectList(conditions, "ConditionId", "ConditionName");
                    ViewData["PatientId"] = patientCondition.PatientId;
                    ViewBag.Message = "Not all the information was entered, Please look below for what's missing.";
                    return View();
                }
                await _pc.UpdatePatientCondition(patientCondition);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Delete([Bind("PatientId, ConditionId, Active")] PatientCondition patientCondition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return RedirectToAction(nameof(Details), new { patientId = patientCondition.PatientId, conditionId = patientCondition.ConditionId });
                }
                await _pc.DeletePatientCondition(patientCondition);
                return RedirectToAction(nameof(IndexForPatient), new { id = patientCondition.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new {patientId = patientCondition.PatientId, conditionId = patientCondition.ConditionId});
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
