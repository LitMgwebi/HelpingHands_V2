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
    }
}
