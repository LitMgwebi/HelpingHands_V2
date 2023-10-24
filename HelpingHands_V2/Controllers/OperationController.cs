using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class OperationController : Controller
    {
        private readonly IOperation _op;

        public OperationController(IOperation op)
        {
            _op = op;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var operation = await _op.GetOperationHours();

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
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var op = await _op.GetOperation(id);

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
        public async Task<IActionResult> Create([Bind("OperationDay, OpenTime, CloseTime, BusinessId, Active")] OperationHour operationHour)
        {
            try
            {
                await _op.AddOperationHours(operationHour);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
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
