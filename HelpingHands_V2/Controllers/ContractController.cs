using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult IndexForUser(int id, string command)
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };
                
                if (command == "patient")
                {
                    contracts = _report.PatientContract(id);
                } 
                else if(command == "current")
                {
                    contracts = _report.NurseAssignedContracts(id);
                } 
                else if (command == "unassigned")
                {
                    contracts = _report.NurseContractType(id, "N");
                } 
                else if (command == "past")
                {
                    contracts = _report.NurseContractType(id, "C");
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
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contract = await _contract.GetContract(id);

                if (contract == null)
                    return NotFound();

                ViewBag.Contract = contract;
                return View();
            }
            catch (Exception ex)
            {
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
                ModelState.Remove("PatientId");
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

                ViewData["CurrentDate"] = DateTime.Now;
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
            try
            {
                if (!ModelState.IsValid)
                {

                    DateTime currentDate = DateTime.Now;
                    var nurses = await _nurse.GetNurses();
                    var wounds = await _wound.GetWounds();
                    var suburbs = await _suburb.GetSuburbs();

                    ViewData["CurrentDate"] = DateTime.Now;
                    ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewBag.Contract = contract;

                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _contract.UpdateContract(contract);
                return RedirectToAction("Dashboard", "Nurse", new {id = contract.NurseId});
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
                return RedirectToAction(nameof(Index));
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
