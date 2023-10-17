using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class WoundController : Controller
    {
        private readonly IWound _wound;

        public WoundController(IWound wound)
        {
            _wound = wound;
        }

        public IActionResult Index()
        {
            try
            {
                var wounds = _wound.GetWounds();

                if (wounds == null)
                {
                    return NotFound();
                }
                ViewBag.Wounds = wounds;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
