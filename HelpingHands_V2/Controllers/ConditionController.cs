using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
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

        public IActionResult Index()
        {
            try
            {
                var condition = _condition.GetConditions();

                if (condition == null)
                {
                    return NotFound();
                }
                ViewBag.Condition = condition;
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

                var condition = _condition.GetCondition(id);

                if (condition == null)
                    return NotFound();

                ViewBag.Condition = condition;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
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
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ConditionName, ConditionDescription, Active")] Condition condition)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _condition.AddCondition(condition);
                    ViewBag.Message = "Record Added successfully;";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Message = "Operation unsuccessful";
                return View();
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
