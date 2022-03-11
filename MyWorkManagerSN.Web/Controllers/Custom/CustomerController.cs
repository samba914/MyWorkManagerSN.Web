
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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult All()
        {

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = new DbManager<User>().Get(u => u.UserId == userId);
            ViewData["devise"] = user.Devise;
            ViewData["userId"] = userId;
            List<Customer> listCustomer = new DbManager<Customer>().GetAll(p => p.UserId == userId);


            return View(listCustomer);
        }
        [HttpPost]
        public JsonResult UpdateAddress(string id, string rue, string ville,string complement,string codepostal, string pays)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Customer customer = new DbManager<Customer>().Get(c => c.UserId == userId && c.ID == id);
                if (customer == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    Address oAdress = new Address { Rue = rue, Ville = ville,Complement=complement, CodePostal = codepostal, PaysCode = pays };
                    customer.Address = oAdress;
                    new CustomerService().UpdateUserAdress(customer);
                    return Json(new { success = true, _acts = new { title = "Adresse mise à jour" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
            
        }
        [HttpGet]
        public JsonResult GetAllCustomerForSub()
        {

            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<SubscriptionWithAmount> listSub = new Service.DbManager<SubscriptionWithAmount>().GetAll(o => o.UserId == userId);
                List<Customer> listCustomer = new List<Customer>();
                if (listSub != null)
                {
                    foreach (SubscriptionWithAmount sb in listSub)
                    {
                        listCustomer.Add(new Service.DbManager<Customer>().GetById(userId, sb.CustomerId));
                    }
                }
                return Json(new { success = true, _acts = new { list = listCustomer } });

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpGet]
        public JsonResult GetAll()
        {
            
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Customer> listCustomer = new DbManager<Customer>().GetAll(p => p.UserId == userId);
                return Json(new { success = true, _acts = new { list = listCustomer } });

            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }

        [HttpPost]
        public JsonResult Add(string name, string surname, string email, string mobile, string address)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                    Customer customer = new Customer { UserId = userId, Name= name, Surname= surname, Email= email, Mobile= mobile, Address=new Address()};
                    string customerId = new DbManager<Customer>().Add(customer);
                    return Json(new { success = true, _acts = new { title = "Client ajouté" , CustomerId= customerId, Name = name, Surname = surname } });
                
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpPost]
        public JsonResult Update(string id, string name, string surname, string email, string mobile)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                Customer customer = new DbManager<Customer>().Get(c => c.UserId == userId && c.ID == id);
                if (customer == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    customer.Name = name;
                    customer.Surname = surname;
                    customer.Email = email;
                    customer.Mobile = mobile;
                    new DbManager<Customer>().Update(customer);
                    return Json(new { success = true, _acts = new { title = "Client mis à jour" } });
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
                Customer customer = new DbManager<Customer>().Get(c => c.UserId == userId && c.ID == id);
                if (customer == null)
                {
                    return Json(new { success = false, _acts = new { title = "Une erreur c'est produit!" } });
                }
                else
                {
                    new DbManager<Customer>().Remove(customer);
                    return Json(new { success = true, _acts = new { title = "Client supprimé" } });
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }
        [HttpGet]
        public JsonResult GetCustomerDetails(string id)
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = new DbManager<Customer>().Get(c => c.UserId == userId && c.ID == id);
                return Json(new { success = true, _acts = new { Name= customer.Name, Surname = customer.Surname} });
            }
            catch(Exception e)
            {
                return Json(new { success = false, _acts = new { title = e.Message } });
            }
        }


        [HttpGet]
        public JsonResult GetAllCustomerFromSub()
        {
            try
            {
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<SubscriptionWithAmount> listSub = new Service.DbManager<SubscriptionWithAmount>().GetAll(o => o.UserId == userId);
                List<string> subListCustomerID = new List<string>();
                if (listSub != null)
                {
                    foreach (string l in subListCustomerID)
                    {
                        subListCustomerID.Add(l);
                    }
                }
                List<Customer> list = new Service.DbManager<Customer>().GetAll(pt => pt.UserId == userId);
                foreach (Customer c in list.ToList())
                {
                    if (subListCustomerID.Contains(c.ID))
                    {
                        list.Remove(c);
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
