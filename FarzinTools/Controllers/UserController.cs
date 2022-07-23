using FarzinTools.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FarzinTools.Controllers
{
    [Authorize(Roles = "admin")] 
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var normalUser = await UserManager.FindByNameAsync("user");

            var result = await UserManager.ChangePasswordAsync(normalUser.Id, model.OldPassword, model.NewPassword);
            
            if (result.Succeeded)
            {
                TempData["alert"] = "کلمه عبور با موفقیت تغییر پیدا کرد";

                return RedirectToAction("ChangePassword", "User");
            }
            
            AddErrors(result);
            
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}