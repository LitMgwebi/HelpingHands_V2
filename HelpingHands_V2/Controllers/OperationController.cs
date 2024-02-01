using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "A")]
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
                var operations = await _op.GetOperationHours();

                if (operations == null)
                {
                    return NotFound();
                }

                OperationsViewModel operationsViewModel = new OperationsViewModel
                {
                    OperationHours = operations
                };

                return View(operationsViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("OperationHoursId")] int? OperationHoursId)
        {
            try
            {
                if (OperationHoursId == null)
                {
                    return NotFound();
                }

                var op = await _op.GetOperation(OperationHoursId);
                var operations = await _op.GetOperationHours();


                if (op == null || operations == null)
                    return NotFound();

                OperationsViewModel operationsViewModel = new OperationsViewModel { 
                    OperationHours = operations,
                    OperationHour = op
                };  

                return View(operationsViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationDay, OpenTime, CloseTime, BusinessId, Active")] OperationHour operationHour)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _op.AddOperationHours(operationHour);
                ViewBag.Message = "Record Added successfully;";
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
        public async Task<IActionResult> Edit([Bind("OperationHoursId, OperationDay, OpenTime, CloseTime, BusinessId, Active")] OperationHour operationHour)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return RedirectToAction(nameof(Index));
                }
                await _op.UpdateOperationHour(operationHour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View(operationHour);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("OperationHoursId")] int OperationHoursId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Index));
                }
                await _op.DeleteOperationHour(OperationHoursId);
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
