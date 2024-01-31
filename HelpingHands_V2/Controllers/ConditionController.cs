using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class ConditionController : Controller
    {
        private readonly ICondition _condition;

        public ConditionController(ICondition condition)
        {
            _condition = condition;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var conditions = await _condition.GetConditions();

                if (conditions == null)
                {
                    return NotFound();
                }

                ConditionsViewModel conditionsViewModel = new ConditionsViewModel
                {
                    Conditions = conditions
                };
                return View(conditionsViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ConditionId")]int? ConditionId)
        {
            try
            {
                if (ConditionId == null)
                {
                    return NotFound();
                }

                var condition = await _condition.GetCondition(ConditionId);
                var conditions = await _condition.GetConditions();

                if (condition == null || conditions == null)
                    return NotFound();

                ConditionsViewModel conditionsViewModel = new ConditionsViewModel
                {
                    Conditions = conditions,
                    Condition = condition
                };

                return View(conditionsViewModel);
            } catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConditionName, ConditionDescription, Active")] Condition condition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(condition);
                }
                await _condition.AddCondition(condition);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View(condition);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ConditionId, ConditionName, ConditionDescription, Active")] Condition condition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Condition = condition;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return RedirectToAction(nameof(Index));
                }
                await _condition.UpdateCondition(condition);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("ConditionId")] int ConditionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Index));
                }
                await _condition.DeleteCondition(ConditionId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
