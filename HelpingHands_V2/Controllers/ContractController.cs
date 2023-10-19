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

        public IActionResult Index()
        {
            try
            {
                var contracts = _contract.GetContracts();

                if(contracts == null)
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
        public IActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var contract = _contract.GetContract(id);

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

        public IActionResult Create()
        {
            try
            {
                var patients = _patient.GetPatients();
                var nurses = _nurse.GetNurses();
                var wounds = _wound.GetWounds();
                var suburbs = _suburb.GetSuburbs();

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
        public IActionResult Create([Bind("ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {
            try
            {
                _contract.AddContract(contract);
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


    }
}
