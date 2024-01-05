using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iText; 
using System.Diagnostics.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

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
                    contracts = await _report.ContractStatus("N");
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
                List<Visit> visits = new List<Visit> { };
                CareContract contract = await _contract.GetContract(id);

                if (contract == null)
                    return NotFound();

                visits = await _report.ContractVisits(contract.ContractId);

                ContractAndVisits cAndV = new ContractAndVisits
                {
                    Contract = contract,
                    Visits = visits
                };

                return View(cAndV);
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
                ViewBag.Message = "Record Added successfully;";
                return RedirectToAction("Dashboard", "Patient", new {id = contract.PatientId});
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

                if (contract == null || patients == null || nurses == null|| wounds == null|| suburbs == null)
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
                    return RedirectToAction("Dashboard", "Nurse", new { id = contract.NurseId });
                } else
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
                    return RedirectToAction(nameof(Index), new {id = HttpContext.User.FindFirst("UserId")!.Value, command = "patient"});
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

        [HttpPost]
        public IActionResult GeneratePDF(ContractAndVisits contractAndVisits)
        {
            var outputFileName = Path.GetTempFileName();
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outputFileName, FileMode.Create, FileAccess.Write)));
            Document document = new Document(pdfDocument);

            try
            {
                Paragraph heading = new Paragraph("Contract");
                heading.SetUnderline(2f, -1f);
                Paragraph contractDetails = new Paragraph(contractAndVisits.Contract!.ContractStatus);
                document.Add(heading);
                document.Add(contractDetails);

                Table table = new Table(2, true);
                IEnumerable<Visit> Visits = contractAndVisits.Visits!;

                Cell cell = new Cell().Add(new Paragraph("Visit Date"));
                cell.SetBold();
                table.AddCell(cell);
                cell = new Cell().Add(new Paragraph("Wound Condition"));
                cell.SetBold();
                table.AddCell(cell);
                foreach (var x in Visits)
                {
                    var visitDate = new Paragraph(x.VisitDate.ToString());
                    var woundCondition = new Paragraph(x.WoundCondition);

                    var column1 = new Cell().Add(visitDate);
                    var column2 = new Cell().Add(woundCondition);
                    table.AddCell(column1);
                    table.AddCell(column2);
                }
                DateTime currentDate = DateTime.Now;
                Paragraph dateParagraph = new Paragraph("Created Date: " + currentDate.ToString("yyyy-MM-dd"));
                dateParagraph.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.LEFT);
                document.Add(dateParagraph);
                document.Add(table);

                Paragraph disclaimer = new Paragraph("Disclaimer: The contents of this pdf are for reference only.");
                document.Add(disclaimer);

                return RedirectToAction(nameof(Details), new {id = contractAndVisits.Contract!.ContractId});
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
                return RedirectToAction(nameof(Details), new { id = contractAndVisits.Contract!.ContractId });
            }
            finally
            {
                document.Close();
                Response.ContentType = "application/pdf";
                Response.AppendTrailer("content-disposition",
                    "attachment;filename=Suppliers.pdf");
                Response.SendFileAsync(outputFileName);
            }
        }
    }
}
