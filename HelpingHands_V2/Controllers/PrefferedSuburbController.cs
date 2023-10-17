using HelpingHands_V2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpingHands_V2.Controllers
{
    public class PrefferedSuburbController : Controller
    {
        private readonly IPrefferedSuburb _ps;

        public PrefferedSuburbController(IPrefferedSuburb ps)
        {
            _ps = ps;
        }
        public IActionResult Index()
        {
            try
            {
                var ps = _ps.GetPrefferedSuburbs();

                if (ps == null)
                {
                    return NotFound();
                }
                ViewBag.PrefferedSuburbs = ps;
                return View();

            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
