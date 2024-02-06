using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.Contracts;

namespace HelpingHands_V2.Controllers
{
    [Authorize(Roles ="A")]
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
                //ViewBag.BusinessInfo = businessInfo;
                return View(businessInfo);

            } catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var businessInfo = await _business.GetBusinessInfo();

                if (businessInfo == null)
                    return NotFound();

                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                return View(businessInfo);
            }
            catch (Exception ex)
            {
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Message = ex.Message;
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BusinessId, OrganizationName, Nponumber, AddressLineOne, AddressLineTwo, SuburbId, ContactNumber, Email, Active")] BusinessInformation bi)
        {
            var suburbs = await _suburb.GetSuburbs();
            try
            {
                ModelState.Remove("Suburb");
                if (!ModelState.IsValid)
                {
                    ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                    ViewBag.Message = $"Not all the information required was entered. Please look below";
                    return View(bi);
                }
                await _business.UpdateBusinessInfo(bi);
                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                ViewData["SuburbId"] = new SelectList(suburbs, "SuburbId", "SuburbName");
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
