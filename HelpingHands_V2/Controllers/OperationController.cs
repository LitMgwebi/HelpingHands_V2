using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                //ViewBag.Operations = operations;
                return View(operations);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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

                //ViewBag.Operation = op;
                return View(op);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
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

        public async Task<IActionResult> Edit(int? id)
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

                //ViewBag.Operation = op;
                return View(op);
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
                    //ViewBag.Operation = operationHour;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(operationHour);
                }
                await _op.UpdateOperationHour(operationHour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View(operationHour);
                //return new JsonResult(new { error = ex.Message });
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
                    return RedirectToAction(nameof(Details), new { id = OperationHoursId });
                }
                await _op.DeleteOperationHour(OperationHoursId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new {id = OperationHoursId});
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
