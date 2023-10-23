using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "N, A, O")]
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
                List<dynamic> assignedContracts = _report.NurseAssignedContracts(id);
                List<object> nextVisit = new List<object> { };
                foreach (var contract in assignedContracts)
                {
                    IEnumerable<dynamic>? contractVisits = _report.ContractVisits(contract.ContractId);

                    var result = DateTime.Compare(contractVisits.Last().VisitDate, currentDate);

                    if (result > 0)
                    {
                        nextVisit.Add(contractVisits.Last());
                    }
                }

                    ViewBag.NextVisit = nextVisit;
                ViewBag.Contracts = (List<object>)assignedContracts;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }


        // GET: Nurses
        public IActionResult Index()
        {
            try
            {
                var nurses = _nurse.GetNurses();
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

        // GET: Nurses/Profile/5
        public IActionResult Profile(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nurse = _nurse.GetNurse(id);
                var user = _user.GetUserById(id);
                if (nurse == null)
                {
                    return NotFound();
                }

                ViewBag.Nurse = nurse;
                ViewBag.User = user;
                return View();
            } catch(Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        // GET: Nurses/Create
        public IActionResult Create(int id)
        {
            ViewData["NurseId"] = id;
            return View();
        }

        // POST: Nurses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NurseId,Grade,Active")] Nurse nurse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nurse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NurseId"] = new SelectList(_context.EndUsers, "UserId", "UserId", nurse.NurseId);
            return View(nurse);
        }

        // GET: Nurses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nurses == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
            {
                return NotFound();
            }
            ViewData["NurseId"] = new SelectList(_context.EndUsers, "UserId", "UserId", nurse.NurseId);
            return View(nurse);
        }

        // POST: Nurses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NurseId,Grade,Active")] Nurse nurse)
        {
            if (id != nurse.NurseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nurse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NurseExists(nurse.NurseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NurseId"] = new SelectList(_context.EndUsers, "UserId", "UserId", nurse.NurseId);
            return View(nurse);
        }

        // GET: Nurses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nurses == null)
            {
                return NotFound();
            }

            var nurse = await _context.Nurses
                .Include(n => n.NurseNavigation)
                .FirstOrDefaultAsync(m => m.NurseId == id);
            if (nurse == null)
            {
                return NotFound();
            }

            return View(nurse);
        }

        // POST: Nurses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nurses == null)
            {
                return Problem("Entity set 'Grp0444HelpingHandsContext.Nurses'  is null.");
            }
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse != null)
            {
                _context.Nurses.Remove(nurse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NurseExists(int id)
        {
            return (_context.Nurses?.Any(e => e.NurseId == id)).GetValueOrDefault();
        }
    }
}
