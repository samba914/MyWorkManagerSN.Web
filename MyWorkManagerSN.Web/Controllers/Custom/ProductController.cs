
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
using MyWorkManagerSN.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;


namespace MyWorkManager.Controllers.Custom
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult All()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            List<Product> listProduct = new DbManager<Product>().GetAll(p=>p.UserId==userId);
            ViewData["userId"] = userId;
            listProduct= listProduct.OrderBy(p => p.Stock).ToList();
            return View(listProduct);
        }
        [HttpGet]
        public JsonResult Get(string id)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Product product = new DbManager<Product>().Get(c => c.UserId == userId && c.ID == id);
                if (product == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produite!" } });
                }
                else
                {

                    return Json(new { success = true, _acts = new { title = "produit", product= product,catLabel= new DbManager<Category>().Get(c => c.UserId == userId && c.ID == product.CategoryId).Label } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Add(string code, string label, string priceTtc,  string priceHt,  int tva, string categoryId, int stock)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Product product= new DbManager<Product>().Get(c => c.UserId == userId && c.Code == code);
                if (product != null)
                {
                    return Json(new { success = false, _acts = new { title = "Une produit avec le même code existe déjà!" } });
                }
                else
                {
                    product = new Product { UserId = userId, Code = code, Label = label, CategoryId = categoryId, PriceHT = double.Parse(priceHt),PriceTtc= double.Parse(priceTtc), TVA = tva , Stock= stock};
                    new DbManager<Product>().Add(product);
                    return Json(new { success = true, _acts = new { title = "produit ajoutée" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Update(string code, string label, string priceTtc, string priceHt,  int tva, string categoryId, int stock)
        {

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Product product = new DbManager<Product>().Get(c => c.UserId == userId && c.Code == code);
                if (product == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    product.Label = label;
                    product.PriceTtc = double.Parse(priceTtc);
                    product.PriceHT = double.Parse(priceHt);
                    product.TVA = tva;
                    product.CategoryId = categoryId;
                    product.Stock = stock;
                    new DbManager<Product>().Update(product);
                    return Json(new { success = true, _acts = new { title = "Produit mis à jour" } });
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
                Product produit = new DbManager<Product>().Get(c => c.UserId == userId && c.ID == id);
                if (produit == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    new DbManager<Product>().Remove(produit);
                    return Json(new { success = true, _acts = new { title = "Produit supprimé" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpGet]
        public JsonResult GetProductFilteredByQuoteLine(string QuoteId)
        {

            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Quote quote = new QuoteService().GetQuoteWithLines(q => q.UserId == userId && q.ID == QuoteId);
                List<string> lineListID = new List<string>();
                if (quote.Lines != null)
                {
                    foreach (QuoteLine l in quote.Lines)
                    {
                        lineListID.Add(l.ProductId);
                    }
                }
                List<Product> list = new DbManager<Product>().GetAll(pt => pt.UserId == userId);
                foreach (Product p in list.ToList())
                {
                    Category cat = new DbManager<Category>().GetById(userId, p.CategoryId);
                    p.Label = cat.Label + " - " + p.Label;
                    if (lineListID.Contains(p.ID))
                    {
                        list.Remove(p);
                    }
                }
                return Json(new { success = true, _acts = new { list = list } });

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpGet]
        public JsonResult GetProsuctFilteredByLine(string OrderId)
        {

            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Order order = new DbManager<Order>().GetOrderWithLines(o => o.UserId == userId && o.ID == OrderId);
                List<string> lineListID = new List<string>();
                if (order.Lines != null)
                {
                    foreach (OrderLine l in order.Lines)
                    {
                        lineListID.Add(l.ProductId);
                    }
                }
                List<Product> list = new DbManager<Product>().GetAll(pt => pt.UserId == userId && pt.Stock > 0);
                foreach(Product p in list.ToList())
                {
                    Category cat =new DbManager<Category>().GetById(userId, p.CategoryId);
                    p.Label = cat.Label + " - " + p.Label;
                    if (lineListID.Contains(p.ID))
                    {
                        list.Remove(p);
                    }
                }
                return Json(new { success = true, _acts = new { list = list } });

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
    }
}