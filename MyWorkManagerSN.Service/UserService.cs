using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class UserService
    {
        public  async void ConfigureAccountOnRegister(IdentityUser user,String userName, String email,UserManager<IdentityUser> userManager)
        {
            AccountOptions aO = new AccountOptions();
            aO.ActiveSubWithAmount = false;
            new DbManager<User>().Add(new User { Devise = "", Username = userName, UserId = user.Id, Email = email, AccountOptions = aO, Address = new Address(), IsTrial = true, DateOfSubscription = DateTime.Now,TrialEndDate=DateTime.Now.AddDays(30), HaveActiveContractOrTrial = true }); ;
            await userManager.AddToRoleAsync(user, GlobalVariableService.RoleActiveAccount);
            PaymentMode paymentModeESP = new PaymentMode { UserId = user.Id, Code = "ESP", Label = "Espèces" };
            PaymentMode paymentModeCB = new PaymentMode { UserId = user.Id, Code = "CB", Label = "Carte Bancaire" };
            Customer customer = new Customer { UserId = user.Id, Name = "Anonyme", Surname = "Client", Email = "", Mobile = "", Address = new Address() };
            new DbManager<Customer>().Add(customer);
            new DbManager<PaymentMode>().Add(paymentModeESP);
            new DbManager<PaymentMode>().Add(paymentModeCB);
        }
        public void UpdateOptions(string userId, string optionProprety,bool value)
        {
            User user = new Service.DbManager<User>().Get(u => u.UserId == userId);
            AccountOptions option = user.AccountOptions;
            PropertyInfo pI = typeof(AccountOptions).GetProperty(optionProprety);
            if (pI != null)
            {
                pI.SetValue(option, value, null);
                user.AccountOptions = option;
                using (var context = new MyEntitiesContext())
                {
                    context.Update(user);
                    context.SaveChanges();
                }
            }
            
        }

        public void UpdateUserTrial(string userId, string date, string userIdentityId)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            User user = new Service.DbManager<User>().Get(u=> u.ID == userId);
            user.TrialEndDate = dateTime;
            if(dateTime > DateTime.Now)
            {
                user.IsTrial = true;
                user.HaveActiveContractOrTrial = true;
            }
            else
            {
                user.IsTrial = false;
                user.HaveActiveContractOrTrial = false;
                if (user.ContractId != null)
                {
                    Contract c = new DbManager<Contract>().GetById(userIdentityId, user.ContractId);
                    if(c != null && c.IsActive)
                    {
                        user.HaveActiveContractOrTrial = true;
                    }
                }
            }
            UpdateUser(user);
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            users = new DbManager<User>().GetAll();
            return users;
        }

        public void UpdateUserAdress(User element)
        {
            UpdateUser(element);
        }

        public void UpdateUser(User element)
        {
            using (var context = new MyEntitiesContext())
            {

                if (element != null)
                {
                    context.Update(element);
                }
                context.SaveChanges();
            }
        }
        
    }
}
