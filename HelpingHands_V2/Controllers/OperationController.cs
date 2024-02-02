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
                OperationsViewModel operationsViewModel = await CreateModel();

                if(operationsViewModel == null)
                    return NotFound();

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

                OperationsViewModel operationsViewModel = await CreateModel(OperationHoursId);

                if (operationsViewModel == null)
                    return NotFound();

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
            OperationsViewModel operationsViewModel = await CreateModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(nameof(Index), operationsViewModel);
                }
                await _op.AddOperationHours(operationHour);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), operationsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("OperationHoursId, OperationDay, OpenTime, CloseTime, BusinessId, Active")] OperationHour operationHour)
        {
            OperationsViewModel operationsViewModel = await CreateModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(nameof(Index), operationsViewModel);
                }
                await _op.UpdateOperationHour(operationHour);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return View(nameof(Index), operationsViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("OperationHoursId")] int OperationHoursId)
        {
            OperationsViewModel operationsViewModel = await CreateModel();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return View(nameof(Index), operationsViewModel);
                }
                await _op.DeleteOperationHour(OperationHoursId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(nameof(Index), operationsViewModel);
            }
        }

        public async Task<OperationsViewModel> CreateModel(int? id = null)
        {
            OperationsViewModel operationsViewModel;
            OperationHour operation;
            IEnumerable<OperationHour> operations;

            operations = await _op.GetOperationHours();
            operation = await _op.GetOperation(id);

            if (operations == null)
            {
                throw new NullReferenceException("Could not retrieve cities from the database");
            }

            operationsViewModel = new OperationsViewModel
            {
                OperationHours = operations,
                OperationHour = operation
            };

            return operationsViewModel;
        }
    }
}
