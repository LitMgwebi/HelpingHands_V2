using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class OperationController : Controller
    {
        private readonly IOperation _op;

        public OperationController(IOperation op)
        {
            _op = op;
        }

        public IActionResult Index()
        {
            try
            {
                var operation = _op.GetOperationHours();

                if (operation == null)
                {
                    return NotFound();
                }
                ViewBag.Operations = operation;
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

                var op = _op.GetOperation(id);

                if (op == null)
                    return NotFound();

                ViewBag.Operation = op;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
