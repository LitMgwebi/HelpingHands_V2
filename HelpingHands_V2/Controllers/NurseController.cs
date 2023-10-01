using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpingHands_V2.Models;
using HelpingHands_V2.Interfaces;

namespace HelpingHands_V2.Controllers
{
    public class NurseController : Controller
    {
        private readonly Grp0444HelpingHandsContext _context;
        private readonly INurse _nurse;
        private readonly IReport _report;

        public NurseController(Grp0444HelpingHandsContext context, INurse nurse, IReport report)
        {
            _nurse = nurse;
            _context = context;
            _report = report;
        }

        public IActionResult Dashboard(int id)
        {
            try
            {
                var assignedContracts = _report.NurseAssignedContracts(id);
                var newContracts = _report.ContractStatus("N");
                var contract = assignedContracts.First();
                IEnumerable<dynamic>? contractVisits = _report.ContractVisits(contract.ContractId);

                ViewBag.Contract = contract;
                ViewBag.ContractVisits = contractVisits.Reverse();
                ViewBag.NewContracts = newContracts;
                return View();
                //return new JsonResult(new { content = assignedConditions });
            } catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }


        // GET: Nurses
        public async Task<IActionResult> Index()
        {
            try
            {
                var nurses =  _nurse.GetNurses();
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

        // GET: Nurses/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Nurses/Create
        public IActionResult Create()
        {
            ViewData["NurseId"] = new SelectList(_context.EndUsers, "UserId", "UserId");
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
