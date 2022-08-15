using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyWorkManagerSN.Web.Controllers.Custom
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public RoleController (RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


      

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
    }
}
