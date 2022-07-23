using FarzinTools.Models;
using FarzinTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FarzinTools.Controllers
{
    [Authorize(Roles = "admin")]
    public class PerformController : Controller
    {
        static readonly eOrganizationEntities1 _context = new eOrganizationEntities1();

        public ActionResult DeletePerform()
        {
            ViewBag.TitleId = new SelectList(_context.Entity_BaseInfo.Where(f => f.TitleCode == 30), "TitleId", "Title");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePerform(DeletePerformViewModel viewModel)
        {
            var messages = new List<string>();

            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        messages.Add(error.ErrorMessage);
                    }
                }

                return Json(new { messages }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var performDate = viewModel.PerformDate.Value;

                var minDate = new DateTime(performDate.Year, performDate.Month, performDate.Day);

                var maxDate = new DateTime(performDate.Year, performDate.Month, performDate.Day, 23, 59, 59, 999);

                var titleId = Convert.ToInt32(viewModel.TitleId);

                var personPerforms = _context.Entity_PersonsPerform.Where(f => f.ProjectCode == titleId && f.PerformDate.Value <= maxDate && f.PerformDate.Value >= minDate);

                int performCount = await personPerforms.CountAsync();

                if (performCount > 0)
                {
                    await personPerforms.ForEachAsync(model =>
                    {
                        _context.Entity_PersonsPerform.Remove(model);
                    });

                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, count = performCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                messages.Add(exception.Message);

                return Json(new { success = false, messages }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}