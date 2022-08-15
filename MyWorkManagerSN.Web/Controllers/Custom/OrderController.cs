
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
//using Model;
using MyWorkManagerSN.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;


namespace MyWorkManagerSN.Controllers.Custom
{
    [Authorize]
    public class OrderController : Controller
    {
        
        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            Dictionary<string,string> keyValuePairs = new Dictionary<string,string>();
            keyValuePairs.Add("Payée", "text-success");
            keyValuePairs.Add("Livrée", "text-success");
            keyValuePairs.Add("En cours", "text-warning");
            keyValuePairs.Add("Annulée", "text-danger");
            ViewData["status"] = keyValuePairs;
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            ViewData["user"] = user;
            List<Order> listOrder = new DbManager<Order>().GetAll(c => c.UserId == userId);
            return View(listOrder.OrderBy(x => x.NumOrder).ToList());
        }
        
        public ActionResult DefaultFacture(string id) 
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            ViewData["user"] = user;
            
            Order order = new DbManager<Order>().GetOrderWithLines(o => o.UserId == userId && o.ID == id);
            if(order.DateCreationInvoice == null)
            {
                order.DateCreationInvoice = DateTime.Now;
            }
            new DbManager<Order>().Update(order);
            ViewData["Customer"] = new DbManager<Customer>().GetById(userId, order.CustomerId);
            return View(order);
        }

            public ActionResult Show(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            Order order = new DbManager<Order>().GetOrderWithLines(o => o.UserId == userId && o.ID==id);
            PaymentMode PaymentMode = null;
            if (order.PaymentModeId != null && order.PaymentModeId!="")
            {
                PaymentMode = new DbManager<PaymentMode>().GetById(userId, order.PaymentModeId);
                ViewData["PaymentMode"] = PaymentMode.Label;
            }
                 
            Dictionary<string, string> states = new Dictionary<string, string>();
            if (order.Status=="En cours"){
                states.Add("Livrée","Livrer");
                states.Add("Annulée","Annuler");
            }
            else if (order.Status == "Payée")
            {
                states.Add("En cours", "En cours");
                states.Add("Livrée", "Livrer");
                states.Add("Annulée", "Annuler");
            }
            else if (order.Status == "Livrée")
            {
                states.Add("Annulée", "Annuler");
            }
            else
            {
                states.Add("En cours", "Réactiver");
            }
            ViewData["listStates"] = states;
            string color = "";
            
            switch (order.Status)
            {
                case "En cours": color="btn-warning";  break;
                case "Payée" :color= "btn-success"; break;
                case "Livrée" : color = "btn-success"; break;
                case "Annulée" :color = "btn-danger"; break;
            }
            ViewData["SoldeAbonnement"] = "";
            if (order.IsOrderSubuscription)
            {
                ViewData["SoldeAbonnement"] = new DbManager<SubscriptionWithAmount>().Get(s => s.UserId == userId && s.CustomerId == order.CustomerId).Amount;
            }
            ViewData["stateColor"] = color;
            ViewData["Customer"] = new DbManager<Customer>().GetById(userId, order.CustomerId);
            return View(order);
        }
        [HttpPost]
        public JsonResult EditOrderLine (string OrderId, string LineId, int Quantity, string Discount)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Order order = new DbManager<Order>().GetOrderWithLines(p => p.UserId == userId && p.ID == OrderId);
                if (order == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else
                {
                    OrderLine line = order.Lines.SingleOrDefault(l => l.ID == LineId);
                    Product product = new DbManager<Product>().Get(p => p.UserId == userId && p.ID == line.ProductId);
                    int previousQuantity = line.Quantity;
                    product.Stock = product.Stock + previousQuantity;
                    if (product.Stock < Quantity)
                    {
                        return Json(new { success = false, _acts = new { title = "La quantité demandée est supérieure au stock disponible!" } });
                    }

                    
                    line.Quantity = Quantity;
                    line.Discount = double.Parse(Discount);
                    double previousHT = line.AmountTotalHT;
                    line.AmountTotalHT = (line.UnitPrice - line.Discount) * Quantity;
                    double previousTTc = line.AmountTotalTTc;
                    line.AmountTotalTTc = Math.Round(line.AmountTotalHT + ((line.AmountTotalHT * line.TVA) / 100), 2);
                    order.AmountTotalHT = order.AmountTotalHT - previousHT + line.AmountTotalHT;
                    order.AmountTotal = order.AmountTotal - previousTTc + line.AmountTotalTTc;
                    new OrderService().UpdateOrderLine(order,line);
                    product.Stock -= Quantity;
                    new DbManager<Product>().Update(product);


                    return Json(new { success = true, _acts = new { title = "Produit ajoutée", newAmountTotal = order.AmountTotal } });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult UpdateState(string id, string etat)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Order o = new DbManager<Order>().GetById(userId, id);
                o.Status = etat;
                if(etat== "Annulée")
                {
                    if (o.IsOrderSubuscription)
                    {
                        SubscriptionWithAmount sub = new DbManager<SubscriptionWithAmount>().Get(s => s.UserId == userId && s.CustomerId == o.CustomerId);
                        sub.Amount += o.AmountPaid;
                        new DbManager<SubscriptionWithAmount>().Update(sub);
                    }
                    
                    o.AmountPaid = 0;

                }
                new DbManager<Order>().Update(o);
                return Json(new { success = true, _acts = new { title = "Etat modifié" } });
            }
            catch(Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message} });
            }
        }

        [HttpPost]
        public JsonResult CreateOrder(string date, string customerId,bool isSubMode )
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string orderId = new OrderService().CreateOrder(userId, date, customerId, isSubMode).ID;
                return Json(new { success = true, _acts = new { title = "Commande ajoutée" , orderId= orderId} });
            }catch(Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message} });
            }
            
        }
        [HttpPost]
        public JsonResult RemoveOrderLine(string OrderId,string LineId) 
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                OrderLine line = new DbManager<OrderLine>().GetById(userId, LineId);
                double amountTotal =new OrderService().RemoveOrderLine(OrderId, line,userId) ;
                return Json(new { success = true, _acts = new { title = "Produit supprimée",newAmountTotal = amountTotal } });
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }

        }

        [HttpPost]
        public JsonResult DoPayment(string OrderId,string PaymentModeID, string AmountPaid)
        {

            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Order order = new DbManager<Order>().Get(p => p.UserId == userId && p.ID == OrderId);
                order.PaymentModeId = PaymentModeID ;
                double paid = order.AmountPaid;
                paid += double.Parse(AmountPaid);
                order.AmountPaid = paid;
                if (paid-order.AmountTotal==0)
                {
                    order.Status = "Payée";
                }
                if (order.IsOrderSubuscription)
                {
                    SubscriptionWithAmount sub = new DbManager<SubscriptionWithAmount>().Get(s => s.UserId == userId && s.CustomerId == order.CustomerId);
                    sub.Amount -= double.Parse(AmountPaid);
                    new DbManager<SubscriptionWithAmount>().Update(sub);
                }
                
                new DbManager<Order>().Update(order);
                return Json(new { success = true, _acts = new { title = "Paiement ajouté", newAmountFacture = paid } });
            }
            catch(Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult UpdateDiscount(string OrderId, int DiscountTVA, string DiscountHT, string DiscountTTC)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Json(new OrderService().UpdateDiscount(userId, OrderId, DiscountTVA, DiscountHT, DiscountTTC));
                
        }
        [HttpPost]
        public JsonResult AddOrderLine(string OrderId, string ProductId, int Quantity, string UnitDiscount, string PriceHt, string PriceTTC)
        {
            
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return Json(new OrderService().AddOrderLine(userId, OrderId, ProductId, Quantity, UnitDiscount, PriceHt, PriceTTC));
           
        }

        /*public JsonResult Add(string code, string label)
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
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        public JsonResult Update(string code, string label)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Category categorie = new DbManager<Category>().Get(c => c.UserId == userId && c.Code == code);
                if (categorie == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
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

        public JsonResult Remove(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Category categorie = new DbManager<Category>().Get(c => c.UserId == userId && c.ID == id);
                if (categorie == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else if (new DbManager<Product>().GetAll(p => p.UserId == userId && p.CategoryId == id).Count() > 0)
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
        }*/

    }
}