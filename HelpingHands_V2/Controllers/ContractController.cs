using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;

namespace HelpingHands_V2.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContract _contract;
        private readonly IPatient _patient;
        private readonly INurse _nurse;
        private readonly IWound _wound;
        private readonly ISuburb _suburb;
        private readonly IReport _report;

        public ContractController(IContract contract, IPatient patient, INurse nurse, IWound wound, ISuburb suburb, IReport report)
        {
            _contract = contract;
            _patient = patient;
            _nurse = nurse;
            _wound = wound;
            _suburb = suburb;
            _report = report;
        }

        [Authorize(Roles = "O, A")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var contracts = await _contract.GetContracts();

                if (contracts == null)
                {
                    return NotFound();
                }
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

        public async Task<IActionResult> IndexForUser(int id, string command)
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };
                
                if (command == "patient")
                {
                    contracts = await _report.PatientContract(id);
                } 
                else if(command == "current")
                {
                    contracts = await _report.NurseAssignedContracts(id);
                } 
                else if (command == "unassigned")
                {
                    contracts = await _report.ContractStatus("N");
                } 
                else if (command == "past")
                {
                    contracts = await _report.NurseContractType(id, "C");
                }

                //if (contracts == null)
                //{
                //    return NotFound();
                //}
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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            List<Visit> visits = new List<Visit> { };
            try
            {
                var contract = await _contract.GetContract(id);
                visits = await _report.ContractVisits(id);

                if (contract == null)
                    return NotFound();

                ViewBag.Contract = contract;
                ViewBag.Visits = visits;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Contract = new { };
                ViewBag.Visits = visits;
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "P")]
        public async Task<IActionResult> Create()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                ViewBag.CurrentDate = DateTime.Now;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
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
        public async Task<IActionResult> Create([Bind("ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {
            try
            {
                ModelState.Remove("StartDate");
                ModelState.Remove("EndDate");
                ModelState.Remove("Nurse");
                ModelState.Remove("Patient");
                ModelState.Remove("Suburb");
                ModelState.Remove("Wound");
                if (!ModelState.IsValid)
                {

                    DateTime currentDate = DateTime.Now;
                    var nurses = await _nurse.GetNurses();
                    var wounds = await _wound.GetWounds();
                    var suburbs = await _suburb.GetSuburbs();

                    ViewBag.CurrentDate = DateTime.Now;
                    ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");

                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _contract.AddContract(contract);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Dashboard", "Patient", new {id = contract.PatientId});
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "O, A, N")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                DateTime currentDate = DateTime.Now;
                var contract = await _contract.GetContract(id);
                var patients = await _patient.GetPatients();
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                if (contract == null || patients == null || nurses == null|| wounds == null|| suburbs == null)
                    return NotFound();

                ViewBag.CurrentDate = DateTime.Now;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Contract = contract;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ContractId, ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {

            DateTime currentDate = DateTime.Now;
            var nurses = await _nurse.GetNurses();
            var wounds = await _wound.GetWounds();
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                ModelState.Remove("Nurse");
                ModelState.Remove("Patient");
                ModelState.Remove("Suburb");
                ModelState.Remove("Wound");
                if (!ModelState.IsValid)
                {

                    ViewBag.CurrentDate = DateTime.Now;
                    ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewBag.Contract = contract;

                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    //return new JsonResult(new { errors, contract });
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _contract.UpdateContract(contract);
                if (HttpContext.User.IsInRole("N"))
                {
                    return RedirectToAction("Dashboard", "Nurse", new { id = contract.NurseId });
                } else
                {
                    return RedirectToAction("NewContracts", "Manager");
                }
            }
            catch (Exception ex)
            {
                ViewBag.CurrentDate = DateTime.Now;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Contract = contract;
                return new JsonResult(new { error = ex.Message, contract });
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "O, A, N")]
        public async Task<IActionResult> Close(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                DateTime currentDate = DateTime.Now;
                var contract = await _contract.GetContract(id);
                var patients = await _patient.GetPatients();
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                if (contract == null || patients == null || nurses == null || wounds == null || suburbs == null)
                    return NotFound();

                ViewBag.CurrentDate = DateTime.Now;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Contract = contract;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close([Bind("ContractId, ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {

            DateTime currentDate = DateTime.Now;
            var nurses = await _nurse.GetNurses();
            var wounds = await _wound.GetWounds();
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                ModelState.Remove("Nurse");
                ModelState.Remove("Patient");
                ModelState.Remove("Suburb");
                ModelState.Remove("Wound");
                if (!ModelState.IsValid)
                {

                    ViewBag.CurrentDate = DateTime.Now;
                    ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewBag.Contract = contract;

                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    //return new JsonResult(new { errors, contract });
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _contract.UpdateContract(contract);
                if (HttpContext.User.IsInRole("N"))
                {
                    return RedirectToAction("Dashboard", "Nurse", new { id = contract.NurseId });
                }
                else
                {
                    return RedirectToAction("NewContracts", "Manager");
                }
            }
            catch (Exception ex)
            {
                ViewBag.CurrentDate = DateTime.Now;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Contract = contract;
                return new JsonResult(new { error = ex.Message, contract });
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("ContractId")] int ContractId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Something went wrong with the delete function. Please hold on.";
                    return RedirectToAction(nameof(Details), new { id = ContractId });
                }
                await _contract.DeleteContract(ContractId);
                if (HttpContext.User.IsInRole("P"))
                {
                    return RedirectToAction(nameof(IndexForUser), new {id = HttpContext.User.FindFirst("UserId")!.Value, command = "patient"});
                } else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new {id = ContractId});
                //return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
