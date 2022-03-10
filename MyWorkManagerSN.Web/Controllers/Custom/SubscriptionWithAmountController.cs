
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
    [Authorize]
    public class SubscriptionWithAmountController : Controller
    {
        
        // GET: SubscriptionWithAmount
        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;

            return View(new DbManager<SubscriptionWithAmount>().GetAll(s => s.UserId == userId));
        }
        [HttpPost]
        public JsonResult Add(string customerId, string amount)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                SubscriptionWithAmount sub = new DbManager<SubscriptionWithAmount>().Get(c => c.UserId == userId);

                sub = new SubscriptionWithAmount { UserId = userId, CustomerId = customerId, Amount = double.Parse(amount) };
                new DbManager<SubscriptionWithAmount>().Add(sub);
                return Json(new { success = true, _acts = new { title = "Abonnement ajouté" } });

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Update(string amount)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                SubscriptionWithAmount sub = new DbManager<SubscriptionWithAmount>().Get(c => c.UserId == userId);
                if (sub == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else
                {
                    sub.Amount = double.Parse(amount);
                    new DbManager<SubscriptionWithAmount>().Update(sub);
                    return Json(new { success = true, _acts = new { title = "Abonnement mise à jour" } });
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
                SubscriptionWithAmount sub = new DbManager<SubscriptionWithAmount>().Get(c => c.UserId == userId && c.ID == id);
                if (sub == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                /*else if (new DbManager<Order>().GetAll(o => o.UserId == userId && o.SubscriptionWithAmountId == id).Count() > 0)
                {
                    return Json(new { success = false, _acts = new { title = "Impossible de supprimer ce mode de paiement car il existe une ou plusieurs commandes avec ce mode de paiement!" } });
                }*/
                else
                {
                    new DbManager<SubscriptionWithAmount>().Remove(sub);
                    return Json(new { success = true, _acts = new { title = "Abonnement supprimé" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }

        
    }
}