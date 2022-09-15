using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Service;

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

        public IActionResult ManageAllUsers()
        {
            var users = new UserService().GetAllUsers();
            return View(users);
        }
       
        [HttpPost]
        public async Task<JsonResult> CretaeRole(string name)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(name);
              //  await _roleManager.
                if (!roleExist)
                {
                    var role = new IdentityRole();
                    role.Name = name;
                    await _roleManager.CreateAsync(role);
                    return Json(new { success = true, _acts = new { title = "Rôle ajouté" } });
                }
                else
                {
                    return Json(new { success = false, _acts = new { title = "Ce rôle existe déjà!" } });
                }
                
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message  }});
            }
            

        }
    }
}
