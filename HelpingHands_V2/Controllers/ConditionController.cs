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
                ConditionsViewModel conditionsViewModel = await CreateModel();

                if (conditionsViewModel == null)
                    return NotFound();

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

                ConditionsViewModel conditionsViewModel = await CreateModel(ConditionId);

                if (conditionsViewModel == null)
                    return NotFound();

                return View(conditionsViewModel);
            } catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConditionName, ConditionDescription, Active")] Condition condition)
        {
            ConditionsViewModel conditionsViewModel = await CreateModel(condition.ConditionId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(nameof(Index), conditionsViewModel);
                }
                await _condition.AddCondition(condition);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), conditionsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ConditionId, ConditionName, ConditionDescription, Active")] Condition condition)
        {
            ConditionsViewModel conditionsViewModel = await CreateModel(condition.ConditionId);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Condition = condition;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(nameof(Index), conditionsViewModel);
                }
                await _condition.UpdateCondition(condition);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), conditionsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("ConditionId")] int ConditionId)
        {
            ConditionsViewModel conditionsViewModel = await CreateModel(ConditionId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return View(nameof(Index), conditionsViewModel);
                }
                await _condition.DeleteCondition(ConditionId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), conditionsViewModel);
            }
        }

        public async Task<ConditionsViewModel> CreateModel(int? id = null)
        {
            Condition condition = await _condition.GetCondition(id);
            IEnumerable<Condition> conditions = await _condition.GetConditions();

            if (conditions == null)
                throw new NullReferenceException("Could not retrieve cities from the database");

            ConditionsViewModel conditionsViewModel = new ConditionsViewModel
            {
                Conditions = conditions,
                Condition = condition
            };

            return conditionsViewModel;
        }
    }
}
