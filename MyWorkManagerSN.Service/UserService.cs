using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class UserService
    {
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
