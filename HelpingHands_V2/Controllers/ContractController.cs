using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;

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
        private readonly IEmailSender _email;

        public ContractController(IContract contract, IPatient patient, INurse nurse, IWound wound, ISuburb suburb, IReport report, IEmailSender email)
        {
            _contract = contract;
            _patient = patient;
            _nurse = nurse;
            _wound = wound;
            _suburb = suburb;
            _report = report;
            _email = email;
        }

        public async Task<IActionResult> Index(int? id, string? command)
        {
            try
            {
                IEnumerable<CareContract> contracts;

                if (command == "patient")
                {
                    contracts = await _report.PatientContract(id);
                }
                else if (command == "current")
                {
                    contracts = await _report.NurseAssignedContracts(id);
                }
                else if (command == "unassigned")
                {
                    contracts = await _report.NurseContractsByGrades(id);
                }
                else if (command == "past")
                {
                    contracts = await _report.NurseContractType(id, "C");
                }
                else
                {
                    contracts = await _contract.GetContracts();
                }

                if (contracts == null)
                {
                    return NotFound();
                }
                return View(contracts);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                CareContract contract = await GetContract(id);

                if (contract == null)
                    return NotFound();

                ContractAndVisit contractAndVisit = new ContractAndVisit
                {
                    Contract = contract
                };

                return View(contractAndVisit);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        [Authorize(Roles = "P")]
        public async Task<IActionResult> Create()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                ViewBag.CurrentDate = DateTime.Now;
                ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractStatus, ContractDate, PatientId, NurseId, WoundId, AddressLineOne, AddressLineTwo, SuburbId, StartDate, EndDate, ContractComment, Active")] CareContract contract)
        {
            DateTime currentDate = DateTime.Now;
            var wounds = await _wound.GetWounds();
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.CurrentDate = currentDate;
                    ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");

                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                await _contract.AddContract(contract);

                string email = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
                string fullName = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
                string username = HttpContext.User.FindFirst("Username")!.Value;

                Message emailMessage = new Message(new string[] { email }, fullName, username, "patient_contract");
                _email.SendEmail(emailMessage);
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Dashboard", "Patient", new { id = contract.PatientId });
            }
            catch (Exception ex)
            {
                ViewBag.CurrentDate = currentDate;
                ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
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

                DateTime currentDate = DateTime.Now;
                var contract = await _contract.GetContract(id);
                var patients = await _patient.GetPatients();
                var nurses = await _nurse.GetNurses();
                var wounds = await _wound.GetWounds();
                var suburbs = await _suburb.GetSuburbs();

                if (contract == null || patients == null || nurses == null || wounds == null || suburbs == null)
                    return NotFound();

                ViewBag.CurrentDate = DateTime.Now;
                ViewData["Nurses"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                return View(contract);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
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

                    ViewBag.CurrentDate = currentDate;
                    ViewData["Nurses"] = new SelectList(nurses, "NurseId", "Fullname");
                    ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                    ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View(contract);
                }

                await _contract.UpdateContract(contract);

                if (HttpContext.User.IsInRole("N"))
                {
                    string email = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
                    string fullName = HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;
                    string username = HttpContext.User.FindFirst("Username")!.Value; ;

                    Message emailMessage = new Message(new string[] { email }, fullName, username, "nurse_contract");
                    _email.SendEmail(emailMessage);
                    return RedirectToAction("Dashboard", "Nurse", new { id = contract.NurseId });
                }
                else
                {
                    return RedirectToAction("NewContracts", "Manager");
                }
            }
            catch (Exception ex)
            {
                ViewBag.CurrentDate = currentDate;
                ViewData["Nurses"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewData["Wounds"] = new SelectList(wounds, "WoundId", "WoundName");
                ViewData["Suburbs"] = new SelectList(suburbs, "SuburbId", "SuburbName");
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
                    return RedirectToAction(nameof(Index), new { id = HttpContext.User.FindFirst("UserId")!.Value, command = "patient" });
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { id = ContractId });
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePDF(int ContractId)
        {
            CareContract contract = await GetContract(ContractId);

            if (contract == null)
                return NotFound();
             
            try
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin((float)0.5, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                        .Height(100)

                            .Text("Hello PDF!")
                            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(x =>
                            {
                                x.Spacing(20);

                                x.Item().Text(Placeholders.LoremIpsum());
                                x.Item().Image(Placeholders.Image(200, 100));
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Page ");
                                x.CurrentPageNumber();
                            });
                    });
                }).GeneratePdf($"C:/Users/lithi/Downloads/{contract.Patient!.FullName}-Contract-{contract.ContractId}.pdf");

               

                return new JsonResult("Working");
            }
            catch (Exception ex)
            {
                return new JsonResult(new { Ass = ex.Message });
            }
        }

        public async Task<CareContract> GetContract(int id)
        {
            List<Visit> visits = new List<Visit> { };
            CareContract contract = await _contract.GetContract(id);

            visits = await _report.ContractVisits(contract.ContractId);

            contract.Visits = visits;

            return contract;
        }
    }
}
