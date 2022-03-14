
using Microsoft.EntityFrameworkCore;
using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class OrderService
    {
        
        public double RemoveOrderLine(string OrderId, OrderLine line, string userId)
        {
            double amountTotal = 0;
            using (var context = new MyEntitiesContext())
            {
                var existingOrder = context.Order
                .Where(p => p.ID == OrderId)
                .Include(p => p.Lines)
                .SingleOrDefault();
                context.Order.Include("Lines").Where(i => i.ID == OrderId).FirstOrDefault();

                if (existingOrder != null)
                {
                    // Update order
                    

                    // Delete lines
                    foreach (var existingLine in existingOrder.Lines.ToList())
                    {
                        if (existingLine.ID == line.ID)
                        {
                            existingOrder.AmountTotalHT -= line.AmountTotalHT;
                            existingOrder.AmountTotal -= line.AmountTotalTTc;
                            amountTotal = existingOrder.AmountTotal;
                            context.Entry(existingOrder).CurrentValues.SetValues(existingOrder);
                            //Product product = new DbManager<Product>().GetById(userId, line.Product_ID);
                            //product.Stock += line.Quantity;
                           // new DbManager<Product>().Update(product);
                            context.OrderLines.Remove(existingLine);
                        }
                    }
                }

                context.SaveChanges();
            }
            return amountTotal;
        }
        public double UpdateOrderLine(Order o,OrderLine line)
        {
            double amountTotal = 0;
            using (var context = new MyEntitiesContext())
            {
                var existingOrder = context.Order
                .Where(p => p.ID == o.ID)
                .Include(p => p.Lines)
                .SingleOrDefault();
                context.Order.Include("Lines").Where(i => i.ID == o.ID).FirstOrDefault();

                if (existingOrder != null)
                {
                    // Update order
                    context.Entry(existingOrder).CurrentValues.SetValues(o);
                    foreach (var existingLine in existingOrder.Lines.ToList())
                    {
                        if (existingLine.ID == line.ID)
                        {
                            existingLine.Quantity = line.Quantity;
                            existingLine.Discount = line.Discount;
                            existingLine.AmountTotalHT = line.AmountTotalHT;
                            existingLine.AmountTotalTTc = line.AmountTotalTTc;
                            break;
                        }
                    }
                }

                context.SaveChanges();
            }
            return amountTotal;
        }
    }
}
