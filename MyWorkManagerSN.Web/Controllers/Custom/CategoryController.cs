
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
using MyWorkManagerSN.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace MyWorkManagerSN.Controllers.Custom
{
    [mwmAuthorize(Roles =GlobalVariableService.RoleActiveAccount)]
    public class CategoryController : Controller
    {
        // GET: Category

        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);
            ViewData["userId"] = userId;
            List<Category> listCat = new DbManager<Category>().GetAll(c=> c.UserId==userId);
            return View(listCat);
        }
        [HttpPost]
        public JsonResult Add(string code, string label)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Category categorie = new DbManager<Category>().Get(c => c.UserId == userId && c.Code == code);
                if (categorie != null)
                {
                    return Json(new { success = false, _acts = new { title = "Une catégorie avec le même code existe déjà!" } });
                }
                else
                {
                    categorie = new Category { UserId = userId, Code = code, Label = label };
                    new DbManager<Category>().Add(categorie);
                    return Json(new { success = true, _acts = new { title = "Catégorie ajoutée" } });
                }
            }catch(Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Update(string code, string label)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Category categorie = new DbManager<Category>().Get(c => c.UserId == userId && c.Code == code);
                if (categorie == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else
                {
                    categorie.Label = label;
                    new DbManager<Category>().Update(categorie);
                    return Json(new { success = true, _acts = new { title = "Catégorie mise à jour" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Remove(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Category categorie = new DbManager<Category>().Get(c => c.UserId == userId && c.ID == id);
                if (categorie == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else if (new DbManager<Product>().GetAll(p => p.UserId == userId && p.CategoryId == id).Count()>0)
                {
                    return Json(new { success = false, _acts = new { title = "Impossible de supprimer cette catégorie car il existe un ou plusieurs produits avec cette catégorie!" } });
                }
                else
                {
                    new DbManager<Category>().Remove(categorie);
                    return Json(new { success = true, _acts = new { title = "Catégorie supprimée" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
    }
}