using CloudinaryDotNet.Actions;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Contracts;
using System.Net.Mail;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles = "O, A")]
    public class ManagerController : Controller
    {
        private readonly IReport _report;
        private readonly IVisit _visit;
        private readonly INurse _nurse;
        private readonly IPatient _patient;
        private readonly IContract _contract;
        private readonly IEndUser _user;
        private readonly IEmailSender _email;
        private readonly ISuburb _suburb;

        public ManagerController(IReport report, IVisit visit, INurse nurse, IContract contract, IPatient patient, IEndUser user, IEmailSender email, ISuburb suburb)
        {
            _report = report;
            _visit = visit;
            _nurse = nurse;
            _contract = contract;
            _patient = patient;
            _user = user;
            _email = email;
            _suburb = suburb;
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                List<Suburb> suburbs = new List<Suburb>();
                List<CareContract> contracts = new List<CareContract>();

                suburbs = await _suburb.GetSuburbs();

                foreach (var suburb in suburbs)
                {
                    contracts = await _report.SuburbContracts(suburb.SuburbId);
                    suburb.CareContracts = contracts;
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
