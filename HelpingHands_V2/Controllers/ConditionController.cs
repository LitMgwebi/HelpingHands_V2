using HelpingHands_V2.Interfaces;
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
    }
}
