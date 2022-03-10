
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using Microsoft.EntityFrameworkCore;
using MyWorkManagerSN.Model;

namespace MyWorkManagerSN.Service
{
    public class DbManager<E> where E: mwmObject
    {
       
                   
        public string Add(E element)
        {
            

            using (var context = new MyEntitiesContext())
            {
                element.ID= System.Guid.NewGuid().ToString();
                context.Set<E>().Add(element);
                context.SaveChanges();
            }
            return element.ID;
        }

        public void Update(E element)
        {
            using (var context = new MyEntitiesContext())
            {
                E el = context.Set<E>().Where(e => e.UserId == element.UserId && e.ID == element.ID).FirstOrDefault();
                if (el != null)
                {
                    context.Entry(el).CurrentValues.SetValues(element);
                }
                context.SaveChanges();
            }
        }
        public void AddOrderLine(Order o, OrderLine oLine)
        {
            using (var context = new MyEntitiesContext())
            {
                var existingOrder=context.Order
                .Where(p => p.ID == o.ID)
                .Include(p => p.Lines)
                .SingleOrDefault();
                context.Order.Include("Lines").Where(i => i.ID == o.ID).FirstOrDefault();
               
                    var existingLine = existingOrder.Lines
                        .Where(c => c.ID == oLine.ID && c.ID != default(string))
                        .SingleOrDefault();

                    if (existingLine != null)
                        // Update child
                        context.Entry(existingLine).CurrentValues.SetValues(oLine);
                    else
                    {
                        // Insert child
                        
                        existingOrder.Lines.Add(oLine);
                    }
                

                context.SaveChanges();
                
            }
        }

        public E GetById(string userId,string id)
        {
            
            using (var context = new MyEntitiesContext())
            {
                E enti = context.Set<E>().FirstOrDefault(e=> e.UserId==userId && e.ID==id);
                if (enti != null)
                {
                    return enti;
                }

            }
            return null;
        }

        public E Get(Expression<Func<E,bool>> predicate)
        {
            
            using (var context = new MyEntitiesContext())
            {
                E enti = context.Set<E>().FirstOrDefault(predicate);
                if (enti != null)
                {
                    return enti;
                }
                
            }
            return null;
        }
        public List<E> GetAll()
        {
            var list = new List<E>();
            using (var context = new MyEntitiesContext())
            {
                 list = context.Set<E>().ToList();
            }
            return list;
        }
        public Order GetOrderWithLines(Expression<Func<Order, bool>> predicate)
        {
            var order = new Order();
            using (var context = new MyEntitiesContext())
            {
                order = context.Order
                .Where(predicate)
                .Include(p => p.Lines)
                .SingleOrDefault();
            }
            return order;
        }
        public List<E> GetAll(Expression<Func<E, bool>> predicate)
        {
            var list = new List<E>();
            using (var context = new MyEntitiesContext())
            {
                list = context.Set<E>().Where(predicate).ToList();
            }
            return list;
        }
        public void Remove(E element)
        {
            using (var context = new MyEntitiesContext())
            {
                context.Set<E>().Remove(context.Set<E>().Find(element.ID));
                context.SaveChanges();
            }
        }


    }
}
