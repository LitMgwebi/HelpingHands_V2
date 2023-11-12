using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "O, A")]
    public class ManagerController : Controller
    {
        private readonly IReport _report;
        private readonly IVisit _visit;

        public ManagerController(IReport report, IVisit visit)
        {
            _report = report;
            _visit = visit;
        }

        public IActionResult Dashboard()
        {
            try
            {
                var newContracts = _report.ContractStatus("N");
                var assignedContracts = _report.ContractStatus("A");
                var visits = _visit.GetVisits();

                if (newContracts == null || assignedContracts == null || visits == null)
                {
                    return NotFound();
                }

                ViewBag.NewContracts = newContracts;
                ViewBag.AssignedContracts = assignedContracts;
                ViewBag.Visits = visits;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult NewContracts()
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };

                contracts = _report.ContractStatus("N");

                ViewBag.Contracts = contracts;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
