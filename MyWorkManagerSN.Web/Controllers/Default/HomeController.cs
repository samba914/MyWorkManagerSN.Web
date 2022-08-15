using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWorkManagerSN.Model;
using MyWorkManagerSN.Service;
using System.Security.Claims;
using System.Diagnostics;
using MyWorkManagerSN.Web.Models;

namespace MyWorkManagerSN.Web.Controllers.Default
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? fromString, string? toString)
        {
            string userId = this.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            //   string userId = this.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            DateTime to = DateTime.Now;
            DateTime from = new DateTime(to.Year, to.Month, 1);
            if (fromString != null && toString != null)
            {
                to = DateTime.Parse(toString);
                from = DateTime.Parse(fromString);
            }

            (List<Order> list, List<CustomOrderLine> listCustom, int totalQuantity, double amountTotalTTc) = getSummaryDetails(from, to);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            ViewData["listLine"] = listCustom;
            ViewData["totalQuantity"] = totalQuantity;
            ViewData["fromDate"] = from.ToString("dd/MM/yyyy");
            ViewData["toDate"] = to.ToString("dd/MM/yyyy");
            ViewData["amountTotalTTc"] = amountTotalTTc;


            return View(list);
        }

        [HttpPost]
        public IActionResult Edit(string to, string from)
        {
            DateTime toDate = DateTime.Parse(to);
            DateTime fromDate = DateTime.Parse(from);

            return RedirectToPage("Index", new { to = to, from = from }); ;
        }

        public (List<Order>, List<CustomOrderLine>,int,double) getSummaryDetails(DateTime from,DateTime to)
        {
            string userId = this.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            List<Order> list = new OrderService().GetAllOrderInTimeRange(userId, from, to);

            Model.User user = new Service.DbManager<Model.User>().Get(u => u.UserId == userId);
            
            list = list.Where(o => o.Status == "Payée" || o.Status == "Livrée").ToList();
            List<OrderLine> listLine = list.SelectMany(o => o.Lines).ToList();
            List<CustomOrderLine> listCustom = new List<CustomOrderLine>();
            int totalQuantity = 0;
            double amountTotalTTc = list.Select(o=>o.AmountTotal).Sum();
            foreach (OrderLine line in listLine)
            {
                if (listCustom.Find(l => l.ProductId == line.ProductId) == null)
                {
                    Product product = new DbManager<Product>().GetById(userId, line.ProductId);
                    Category cat = new DbManager<Category>().GetById(userId, product.CategoryId);
                    listCustom.Add(new CustomOrderLine() { ProductId = line.ProductId, Quantity = 0, Designation = cat.Label + " - " + product.Label });

                }
                totalQuantity += line.Quantity;
                listCustom.Find(l => l.ProductId == line.ProductId).Quantity += line.Quantity;
            }

            listCustom = listCustom.OrderByDescending(p => p.Quantity).ToList();

            return (list,listCustom,totalQuantity, amountTotalTTc);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}