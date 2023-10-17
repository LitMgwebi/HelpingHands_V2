using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IBusiness business;

        public BusinessController(IBusiness business)
        {
            this.business = business;
        }
        public IActionResult Index()
        {
            try
            {
                var businessInfo = business.GetBusinessInfo();

                if(businessInfo == null)
                {
                    return NotFound();
                }
                ViewBag.BusinessInfo = businessInfo;
                return View();

            } catch (Exception ex)
            {
                return new JsonResult(new {error = ex.Message});
            }
        }
    }
}
