using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Service;
using System.Security.Claims;

namespace MyWorkManagerSN.Web.Controllers.Custom
{
    public class UserController : Controller
    {
        public JsonResult UpdateOptions(string optionProprety, bool value)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                new UserService().UpdateOptions(userId, optionProprety, value);

                return Json(new { success = true, _acts = new { title = "Mise à jour effectuée" } });
            }

            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }


        }
    }
}
