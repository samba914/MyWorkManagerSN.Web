using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
using MyWorkManagerSN.Service;
using System.Security.Claims;

namespace MyWorkManagerSN.Web.Controllers.Custom
{
    [Authorize]
    public class ProfileController : Controller
    {
        
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProfileController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Show()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new Service.DbManager<User>().Get(u => u.UserId == userId);
            var imageUrl = "";
            if (user.ProfileImagePath != null)
            {
                imageUrl = (user.ProfileImagePath);
            }
            ViewData["user"] = user;
            ViewData["userId"] = userId;
            ViewData["imageUrl"] = imageUrl;
            return View();
        }
        [HttpPost]
        public ActionResult ChangeProfileData(string Mobile, string CompanyName, string Devise)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);
            user.Mobile = Mobile;
            user.CompanyName = CompanyName;
            user.Devise = Devise;
            new DbManager<User>().Update(user);
            return RedirectToAction("Show", "Profile");
        }
        [HttpPost]
        public ActionResult UpdateProfilImage()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);

            string wwwRootPath = _hostEnvironment.WebRootPath;

            IFormFile file  = Request.Form.Files["profile_photo"];
            string fileName = Path.GetFileNameWithoutExtension(Request.Form.Files["profile_photo"].FileName);
            string extension = Path.GetExtension(Request.Form.Files["profile_photo"].FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            user.ProfileImagePath = fileName;
            string path = Path.Combine(wwwRootPath+ "/image",fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                 user.ProfileImageFile = file;
                 user.ProfileImageFile.CopyTo(fileStream);
            }

            new DbManager<User>().Update(user);
            return RedirectToAction("Show", "Profile");
        }
        public JsonResult UpdateShowPictureIncoice(string id, bool value)
        {
            
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = new DbManager<User>().Get(c => c.UserId == userId && c.ID == id);
                if (user == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    user.ShowImageOnInvoice = value;    
                    new UserService().UpdateUserAdress(user);
                    return Json(new { success = true, _acts = new { title = "Mise à jour effectuée" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult UpdateAddress(string id, string rue, string ville, string complement, string codepostal, string pays)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                User user = new DbManager<User>().Get(c => c.UserId == userId && c.ID == id);
                if (user == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    Address oAdress = new Address { Rue = rue, Ville = ville, Complement = complement, CodePostal = codepostal, PaysCode = pays };
                    user.Address = oAdress;
                    new UserService().UpdateUserAdress(user);
                    return Json(new { success = true, _acts = new { title = "Adresse mise à jour" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }

        }
    }
}
