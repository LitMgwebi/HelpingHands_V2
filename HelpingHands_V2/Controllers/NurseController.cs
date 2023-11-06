﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;

namespace HelpingHands_V2.Controllers
{
    //[Authorize(Roles = "N, A, O")]
    public class NurseController : Controller
    {
        private readonly Grp0444HelpingHandsContext _context;
        private readonly INurse _nurse;
        private readonly IReport _report;
        private readonly IEndUser _user;

        public NurseController(Grp0444HelpingHandsContext context, INurse nurse, IReport report, IEndUser user)
        {
            _nurse = nurse;
            _context = context;
            _report = report;
            _user = user;
        }

        public IActionResult Dashboard(int id)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                List<dynamic> assignedContracts = new List<dynamic> { };
                List<dynamic> unassignedContracts = new List<dynamic> { };
                List<dynamic> contractVisits = new List<dynamic> { };
                List<dynamic> nextVisit = new List<dynamic> { };

                assignedContracts = _report.NurseAssignedContracts(id);
                unassignedContracts = _report.ContractStatus("N");
                if (assignedContracts.Count > 0)
                {
                    foreach (var contract in assignedContracts)
                    {
                        contractVisits = _report.ContractVisits(contract.ContractId);

                        if (contractVisits.Count > 0)
                        {
                            foreach (var visit in contractVisits)
                            {
                                nextVisit.Add(visit);
                            }
                        }
                    }
                }
                if (nextVisit.Count > 0)
                {
                    for (int i = 0; i < nextVisit.Count - 1; i++)
                        for (int j = 0; j < nextVisit.Count - i - 1; j++)
                            if (nextVisit[j].VisitDate > nextVisit[j + 1].VisitDate)
                            { 
                                var tempVar = nextVisit[j];
                                nextVisit[j] = nextVisit[j + 1];
                                nextVisit[j + 1] = tempVar;
                            }
                }
                ViewBag.UnassignedContracts = unassignedContracts;
                ViewBag.NextVisit = nextVisit;
                ViewBag.Contracts = assignedContracts;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var nurses = await _nurse.GetNurses();
                if (nurses == null)
                {
                    return NotFound();
                }
                ViewBag.Nurses = nurses;
                return View(nurses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public async Task<IActionResult> Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nurse = await _nurse.GetNurse(id);
                var user = await _user.GetUserById(id);
                if (nurse == null)
                {
                    return NotFound();
                }

                ViewBag.Nurse = nurse;
                ViewBag.User = user;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult Create(int id)
        {
            try
            {
                ViewData["NurseId"] = id;
                return View();
            }
            catch (Exception ex) { return new JsonResult(new { error = ex.Message }); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseId,Grade,Active")] Nurse nurse)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _nurse.AddNurse(nurse);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction(nameof(Dashboard), new { id = nurse.NurseId });
            }
            catch (Exception ex) { return new JsonResult(new { error = ex.Message }); }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nurse = await _nurse.GetNurse(id);
                var user = await _user.GetUserById(id);
                if (nurse == null)
                {
                    return NotFound();
                }

                ViewBag.Nurse = nurse;
                ViewBag.User = user;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("NurseId,Grade,Active")] Nurse nurse)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Model state not valid";
                    return View();
                }
                await _nurse.UpdateNurse(nurse);
                return RedirectToAction(nameof(Profile), new { id = nurse.NurseId });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("NurseId")] int NurseId)
        {
            try
            {
                await _nurse.DeleteNurse(NurseId);
                await _user.DeleteUser(NurseId);
                await HttpContext.SignOutAsync();
                return Redirect("/");
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }


        public IActionResult VisitRange()
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
        public IActionResult VisitRange(int NurseId, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var visitRange = _report.NurseVisitRange(NurseId, StartDate, EndDate);
                ViewBag.VisitRange = visitRange;
                return View();
            } catch(Exception ex)
            {
                return new JsonResult(new {error= ex.Message});
            }
        }

    }
}
