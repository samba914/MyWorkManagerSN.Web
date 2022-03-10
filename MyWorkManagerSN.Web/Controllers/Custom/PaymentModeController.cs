
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
    public class PaymentModeController : Controller
    {
        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<PaymentMode> listPM = new DbManager<PaymentMode>().GetAll(pm => pm.UserId == userId);
            return View(listPM);
        }
        [HttpPost]
        public JsonResult Add(string code, string label)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                PaymentMode paymentMode = new DbManager<PaymentMode>().Get(c => c.UserId == userId && c.Code == code);
                if (paymentMode != null)
                {
                    return Json(new { success = false, _acts = new { title = "Un mode de paiement avec le même code existe déjà!" } });
                }
                else
                {
                    paymentMode = new PaymentMode { UserId = userId, Code = code, Label = label };
                    new DbManager<PaymentMode>().Add(paymentMode);
                    return Json(new { success = true, _acts = new { title = "Mode de paiement ajouté" } });
                }
            }
            catch (Exception e)
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
                PaymentMode paymentMode = new DbManager<PaymentMode>().Get(c => c.UserId == userId && c.Code == code);
                if (paymentMode == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else
                {
                    paymentMode.Label = label;
                    new DbManager<PaymentMode>().Update(paymentMode);
                    return Json(new { success = true, _acts = new { title = "Mode de paiement mis à jour" } });
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
                PaymentMode paymentMode = new DbManager<PaymentMode>().Get(c => c.UserId == userId && c.ID == id);
                if (paymentMode == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else if (new DbManager<Order>().GetAll(o => o.UserId == userId && o.PaymentModeId == id).Count() > 0)
                {
                    return Json(new { success = false, _acts = new { title = "Impossible de supprimer ce mode de paiement car il existe une ou plusieurs commandes avec ce mode de paiement!" } });
                }
                else
                {
                    new DbManager<PaymentMode>().Remove(paymentMode);
                    return Json(new { success = true, _acts = new { title = "Mode de paiement supprimé" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
    }
}