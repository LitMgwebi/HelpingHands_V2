﻿using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Contracts;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "O, A")]
    public class ManagerController : Controller
    {
        private readonly IReport _report;
        private readonly IVisit _visit;
        private readonly INurse _nurse;
        private readonly IContract _contract;

        public ManagerController(IReport report, IVisit visit, INurse nurse, IContract contract)
        {
            _report = report;
            _visit = visit;
            _nurse = nurse;
            _contract = contract;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                List<dynamic> newContracts = new List<dynamic> { };
                List<dynamic> assignedContracts = new List<dynamic> { };
                List<dynamic> visits = new List<dynamic> { };

                newContracts = await _report.ContractStatus("N");
                assignedContracts = await _report.ContractStatus("A");

                if (newContracts == null || assignedContracts == null)
                {
                    return NotFound();
                }

                ViewBag.NewContracts = newContracts;
                ViewBag.AssignedContracts = assignedContracts;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> NewContracts()
        {
            try
            {
                List<dynamic> contracts = new List<dynamic> { };

                contracts = await _report.ContractStatus("N");

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

        public async Task<IActionResult> VisitRange()
        {
            List<dynamic> visitRange = new List<dynamic> { };
            try
            {
                var nurses = await _nurse.GetNurses();
                ViewBag.VisitRange = visitRange;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
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
        public async Task<IActionResult> VisitRange(int NurseId, DateTime StartDate, DateTime EndDate)
        {
            List<dynamic> visitRange = new List<dynamic> { };
            var nurses = await _nurse.GetNurses();
            ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.VisitRange = visitRange;
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                visitRange = await _report.NurseVisitRange(NurseId, StartDate, EndDate);
                ViewBag.VisitRange = visitRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.VisitRange = visitRange;
                ViewData["NurseId"] = new SelectList(nurses, "NurseId", "Fullname");
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public IActionResult ContractRange()
        {
            List<dynamic> contractRange = new List<dynamic> { };
            try
            {
                ViewBag.ContractRange = contractRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ContractRange = contractRange;
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContractRange(DateTime StartDate, DateTime EndDate)
        {
            List<dynamic> contractRange = new List<dynamic> { };
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                contractRange = await _report.CareVisits(StartDate, EndDate);
                ViewBag.ContractRange = contractRange;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ContractRange = contractRange;
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> ContractDetails(int? id)
        {
            List<dynamic> nurses = new List<dynamic> { };
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
                ViewBag.Nurses = nurses;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Contract = new { };
                ViewBag.Message = ex.Message;
                return View();
                //return new JsonResult(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContractDetails(int ContractId)
        {
            List<dynamic> nurses = new List<dynamic> { };
            var contract = await _contract.GetContract(ContractId);
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    ViewBag.Contract = contract;
                    ViewBag.Message = $"Not all the information required was entered. Please look below.";
                    return View();
                }
                nurses = await _report.AvailableNurses(ContractId);
                ViewBag.Nurses = nurses;
                ViewBag.Contract = contract;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Nurses = nurses;
                ViewBag.Contract = contract;
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        //public void GeneratePDF()
        //{
        //    var outputFileName = Path.GetTempFileName();
        //    PdfDocument pdfDocument = new PdfDocument(new PdfWriter(new FileStream(outputFileName, FileMode.Create, FileAccess.Write)));
        //    Document document = new Document(pdfDocument);

        //    try
        //    {
        //        Paragraph heading = new Paragraph("REGEX Suppliers");
        //        heading.SetUnderline(0.5f, -1f);
        //        document.Add(heading);

        //        Table table = new Table(2, true);
        //        List<TblSupplier> Suppliers = _userService.GetSuppliers();

        //        Cell cell = new Cell().Add(new Paragraph("Supplier Name"));
        //        cell.SetBold();
        //        table.AddCell(cell);
        //        cell = new Cell().Add(new Paragraph("Supplier Email"));
        //        cell.SetBold();
        //        table.AddCell(cell);
        //        foreach (var x in Suppliers)
        //        {
        //            var SuppName = new Paragraph(x.SupplierName);
        //            var SuppEmail = new Paragraph(x.SupplierEmail);

        //            var column1 = new Cell().Add(SuppName);
        //            var column2 = new Cell().Add(SuppEmail);
        //            table.AddCell(column1);
        //            table.AddCell(column2);
        //        }
        //        DateTime currentDate = DateTime.Now;
        //        Paragraph dateParagraph = new Paragraph("Created Date: " + currentDate.ToString("yyyy-MM-dd"));
        //        dateParagraph.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.LEFT);
        //        document.Add(dateParagraph);
        //        document.Add(table);

        //        Paragraph disclaimer = new Paragraph("Disclaimer: The contents of this pdf are for reference only.");
        //        document.Add(disclaimer);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        document.Close();
        //        Response.ContentType = "application/pdf";
        //        Response.AppendTrailer("content-disposition",
        //            "attachment;filename=Suppliers.pdf");
        //        Response.SendFileAsync(outputFileName);
        //    }
        //}
    }
}
