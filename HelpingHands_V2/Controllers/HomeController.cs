using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HelpingHands_V2.Interfaces;

namespace HelpingHands_V2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusiness _business;
        private readonly IOperation _operation;

        public HomeController(ILogger<HomeController> logger, IBusiness business, IOperation operation)
        {
            _logger = logger;
            _business = business;
            _operation = operation;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var business = await _business.GetBusinessInfo();
                var operation = await _operation.GetOperationHours();

                if (business == null || operation == null)
                {
                    return NotFound();
                }

                ViewBag.Business = business;
                ViewBag.Operation = operation;
                return View();
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}