using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
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

        public ContractController(IContract contract, IPatient patient, INurse nurse, IWound wound, ISuburb suburb)
        {
            _contract = contract;
            _patient = patient;
            _nurse = nurse;
            _wound = wound;
            _suburb = suburb;
        }

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
                return new JsonResult(new { error = ex.Message, ex.Source });
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
                return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var patients = await _patient.GetPatients();
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                ViewData["PatientId"] = new SelectList(patients, "PatientId", "PatientId");
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "NurseId");
                ViewData["WoundId"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {
            try
            {
                await _contract.AddContract(contract);
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

                var contract = await _contract.GetContract(id);
                var patients = await _patient.GetPatients();
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                if (contract == null || patients == null || nurses == null|| wounds == null|| suburbs == null)
                    return NotFound();

                ViewData["PatientId"] = new SelectList(patients, "PatientId", "PatientId");
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "NurseId");
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
                //return new JsonResult(new { content = contract });
                await _contract.UpdateContract(contract);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("ContractId")] int ContractId)
        {
            try
            {
                await _contract.DeleteContract(ContractId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
