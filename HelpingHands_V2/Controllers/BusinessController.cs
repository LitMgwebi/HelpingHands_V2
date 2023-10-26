using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpingHands_V2.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IBusiness _business;
        private readonly ISuburb _suburb;

        public BusinessController(IBusiness business, ISuburb suburb)
        {
            _business = business;
            _suburb = suburb;
        }
        public async Task<IActionResult> Details()
        {
            try
            {
                var businessInfo = await _business.GetBusinessInfo();

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

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var businessInfo = await _business.GetBusinessInfo();
                var suburbs = await _suburb.GetSuburbs();

                if (businessInfo == null)
                    return NotFound();

                ViewBag.BusinessInfo = businessInfo;
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
        public async Task<IActionResult> Edit([Bind("BusinessId, OrganizationName, Nponumber, AddressLineOne, AddressLineTwo, SuburbId, ContactNumber, Email, Active")] BusinessInformation bi)
        {
            try
            {
                await _business.UpdateBusinessInfo(bi);
                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {

                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}
