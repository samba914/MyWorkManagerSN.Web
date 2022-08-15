using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
using MyWorkManagerSN.Service;
using System.Security.Claims;

namespace MyWorkManagerSN.Web.Controllers.Custom
{
    [Authorize]
    public class QuoteController : Controller
    {
        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            ViewData["user"] = user;
            List<Quote> listQuote = new DbManager<Quote>().GetAll(c => c.UserId == userId);
            return View(listQuote.OrderBy(x => x.NumQuote).ToList());
        }

        public ActionResult Show(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            Quote quote = new QuoteService().GetQuoteWithLines(o => o.UserId == userId && o.ID == id);

            Dictionary<string, string> states = new Dictionary<string, string>();
            if (quote.Status == "En cours")
            {
                states.Add("Perdu", "Perdu");
                states.Add("Gagné", "Gagné");
                
            }
            else
            {
                states.Add("En cours", "Réactiver");
            }
            ViewData["listStates"] = states;
            string color = "";

            switch (quote.Status)
            {
                case "En cours": color = "btn-warning"; break;
                case "Gagné": color = "btn-success"; break;
                case "Perdu": color = "btn-danger"; break;
            }
            if (quote.Order_Id != null)
            {
                ViewData["Order"] = new DbManager<Order>().GetById(userId, quote.Order_Id);
            }
            ViewData["SoldeAbonnement"] = "";
            ViewData["stateColor"] = color;
            ViewData["Customer"] = new DbManager<Customer>().GetById(userId, quote.CustomerId);
            return View(quote);
        }
        public ActionResult DefaultDevis(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            ViewData["user"] = user;

            Quote quote = new QuoteService().GetQuoteWithLines(o => o.UserId == userId && o.ID == id);

            new DbManager<Quote>().Update(quote);
            ViewData["Customer"] = new DbManager<Customer>().GetById(userId, quote.CustomerId);
            return View(quote);
        }

        [HttpPost]
        public JsonResult AddQuoteLine(string QuoteId, string ProductId, int Quantity, string UnitDiscount, string PriceHt, string PriceTTC)
        {

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Json(new QuoteService().AddQuoteLine(userId, QuoteId, ProductId, Quantity, UnitDiscount, PriceHt, PriceTTC));

        }

        [HttpPost]
        public JsonResult RemoveQuoteLine(string QuoteId, string LineId)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                QuoteLine line = new DbManager<QuoteLine>().GetById(userId, LineId);
                double amountTotal = new QuoteService().RemoveQuoteLine(QuoteId, line, userId);
                return Json(new { success = true, _acts = new { title = "Produit supprimé", newAmountTotal = amountTotal } });
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }

        }

        [HttpPost]
        public JsonResult CreateQuote(string date, string customerId)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string quoteId = new QuoteService().CreateQuote(userId, date, customerId);
                return Json(new { success = true, _acts = new { title = "Devis ajouté", quoteId = quoteId } });
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }

        }

        [HttpPost]
        public JsonResult EditQuoteLine(string QuoteId, string LineId, int Quantity, string Discount)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Quote quote = new QuoteService().GetQuoteWithLines(p => p.UserId == userId && p.ID == QuoteId);
                if (quote == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                else
                {
                    QuoteLine line = quote.Lines.SingleOrDefault(l => l.ID == LineId);
                    int previousQuantity = line.Quantity;
                    line.Quantity = Quantity;
                    line.Discount = double.Parse(Discount);
                    double previousHT = line.AmountTotalHT;
                    line.AmountTotalHT = (line.UnitPrice - line.Discount) * Quantity;
                    double previousTTc = line.AmountTotalTTc;
                    line.AmountTotalTTc = Math.Round(line.AmountTotalHT + ((line.AmountTotalHT * line.TVA) / 100), 2);
                    quote.AmountTotalHT = quote.AmountTotalHT - previousHT + line.AmountTotalHT;
                    quote.AmountTotal = quote.AmountTotal - previousTTc + line.AmountTotalTTc;
                    new QuoteService().UpdateQuoteLine(quote, line);


                    return Json(new { success = true, _acts = new { title = "Produit modifié", newAmountTotal = quote.AmountTotal } });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult CreateOrder(string QuoteId)
        {
            string orderId = "";
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Quote quote = new QuoteService().GetQuoteWithLines(p => p.UserId == userId && p.ID == QuoteId);
                Order order = new OrderService().CreateOrder(userId, DateTime.Now.ToString("dd/MM/yyyy"), quote.CustomerId, false);
                orderId = order.ID;
                foreach (QuoteLine l in quote.Lines)
                {
                    new OrderService().AddOrderLine(userId, order.ID, l.ProductId, l.Quantity, l.Discount.ToString(), l.AmountTotalHT.ToString(), l.AmountTotalTTc.ToString());
                }
                new OrderService().UpdateDiscount(userId, order.ID, quote.DiscountTVA, quote.DiscountHT.ToString(), quote.DiscountTTC.ToString());
                quote.Order_Id= orderId;
                new DbManager<Quote>().Update(quote);
                return Json(new { success = true, _acts = new { title = "Commande créée", orderID = orderId } });
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message, orderID = orderId } });
            }
        }

        [HttpPost]
        public JsonResult UpdateDiscount(string QuoteId, int DiscountTVA, string DiscountHT, string DiscountTTC)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Quote quote = new DbManager<Quote>().Get(p => p.UserId == userId && p.ID == QuoteId);
                if (quote == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur s'est produite!" } });
                }
                double previousTTC = quote.DiscountTTC;
                double previousHT = quote.DiscountHT;
                quote.DiscountTTC = double.Parse(DiscountTTC);
                quote.DiscountHT = double.Parse(DiscountHT);
                quote.DiscountTVA = DiscountTVA;
                quote.AmountTotal = quote.AmountTotal + previousTTC - quote.DiscountTTC;
                quote.AmountTotalHT = quote.AmountTotalHT + previousHT - quote.DiscountHT;
                new DbManager<Quote>().Update(quote);
                return Json(new { success = true, _acts = new { title = "Modification effectuée" } });
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
                Quote o = new DbManager<Quote>().GetById(userId, id);
                o.Status = etat;
                new DbManager<Quote>().Update(o);
                return Json(new { success = true, _acts = new { title = "Etat modifié" } });
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
    }
}
