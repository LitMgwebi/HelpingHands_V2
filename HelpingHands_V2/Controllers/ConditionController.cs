﻿using HelpingHands_V2.Interfaces;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var condition = await _condition.GetConditions();

                if (condition == null)
                {
                    return NotFound();
                }
                ViewBag.Conditions = condition;
                return View();

            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
                ViewBag.Message = ex.Message;
                return View();
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

                var condition = await _condition.GetCondition(id);

                if (condition == null)
                    return NotFound();

                ViewBag.Condition = condition;
                return View();
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { error = ex.Message });
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
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing:";

                    return new JsonResult(new { errors });
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
                return View();
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

                var condition = await _condition.GetCondition(id);

                if (condition == null)
                    return NotFound();

                ViewBag.Condition = condition;
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
        public async Task<IActionResult> Edit([Bind("ConditionId, ConditionName, ConditionDescription, Active")] Condition condition)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Condition = condition;
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return View(condition);
                }
                await _condition.UpdateCondition(condition);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Delete([Bind("ConditionId")] int ConditionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information was entered. We found that you are missing: ${errors}";
                    return RedirectToAction(nameof(Details), new { id = ConditionId });
                }
                await _condition.DeleteCondition(ConditionId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { id = ConditionId });
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
