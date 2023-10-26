﻿using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class SuburbController : Controller
    {
        private readonly ISuburb _suburb;
        private readonly ICity _city;

        public SuburbController(ISuburb suburb, ICity city)
        {
            _suburb = suburb;
            _city = city;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var suburbs = await _suburb.GetSuburbs();

                if (suburbs == null)
                {
                    return NotFound();
                }
                ViewBag.Suburbs = suburbs;
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

                var suburb = await _suburb.GetSuburb(id);

                if (suburb == null)
                    return NotFound();

                ViewBag.Suburb = suburb;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var cities = await _city.GetCities();
                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SuburbName, PostalCode, CityId, Active")] Suburb suburb)
        {
            try
            {
                await _suburb.AddSuburb(suburb);
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

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cities = await _city.GetCities();
                var suburb = await _suburb.GetSuburb(id);

                if (suburb == null)
                    return NotFound();

                ViewData["CityId"] = new SelectList(cities, "CityId", "CityName");
                ViewBag.Suburb = suburb;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("SuburbId, SuburbName, PostalCode, CityId, Active")] Suburb suburb)
        {
            try
            {
                await _suburb.UpdateSuburb(suburb);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("SuburbId")] int SuburbId)
        {
            try
            {
                await _suburb.DeleteSuburb(SuburbId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
