using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Service;
using System.Xml.Linq;

namespace MyWorkManagerSN.Web.Controllers.Custom
{
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public RoleController (RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


      

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult ManageAllUsers()
        {
            var users = new UserService().GetAllUsers();
            var roles = _roleManager.Roles.ToList();
            ViewData["listRoles"]=roles;
            return View(users);
        }
        [HttpGet]
        public JsonResult GetUserRoles(string userId)
        {
            
            try
            {
                IdentityUser user = _userManager.FindByIdAsync(userId).Result;
                
                if (user != null)
                {
                    var roles = _userManager.GetRolesAsync(user).Result;
                    return Json(new { success = true, _acts = new { roles = roles } });
                }
                else
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produit!" } });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }

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
        [HttpPost]
        public async Task<JsonResult> UpdateUserRoles(string userId, string[] listRoles)
        {
            try
            {
                IdentityUser user = _userManager.FindByIdAsync(userId).Result;
                foreach(string roleName in listRoles)
                {
                    await _userManager.AddToRoleAsync(user,roleName);
                }
                return Json(new { success = true, _acts = new { title = "Autorisations modifiées" } });
                

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }


        }
    }
}
