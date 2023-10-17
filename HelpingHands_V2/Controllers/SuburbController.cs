using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class SuburbController : Controller
    {
        private readonly ISuburb suburb;

        public SuburbController(ISuburb suburb)
        {
            this.suburb = suburb;
        }

        public IActionResult Index()
        {
            try
            {
                var suburbs = suburb.GetSuburbs();

                if (suburbs == null)
                {
                    return NotFound();
                }
                ViewBag.Suburbs = suburbs;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
