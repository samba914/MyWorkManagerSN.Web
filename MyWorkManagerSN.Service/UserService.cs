using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class UserService
    {
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

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            users = new DbManager<User>().GetAll();
            return users;
        }

        public void UpdateUserAdress(User element)
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
