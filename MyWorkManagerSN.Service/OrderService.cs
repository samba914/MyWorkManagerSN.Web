
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class OrderService
    {
        public Order CreateOrder(string userId, string date, string customerId, bool isSubMode)
        {
                DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                Order o = new Order();
                o.Status = "En cours";
                o.CustomerId = customerId;
                o.AmountPaid = 0;
                o.AmountTotal = 0;
                o.AmountTotalHT = 0;
                o.DateCreation = dateTime;
                o.DiscountHT = 0;
                o.DiscountTTC = 0;
                o.DiscountTTC = 0;
                o.UserId = userId;
                o.IsOrderSubuscription = isSubMode;
                o.Lines = new List<OrderLine>();
                List<Order> listOrder = new DbManager<Order>().GetAll(obj => obj.UserId == userId);
                if (listOrder.Count > 0)
                {
                    o.NumOrder = listOrder.OrderBy(x => x.NumOrder).ToList().Last().NumOrder + 1;
                }
                else
                {
                    o.NumOrder = 1;
                }
                o.ID = new DbManager<Order>().Add(o);
                return o;

        }
        public Object AddOrderLine(string userId,string OrderId, string ProductId, int Quantity, string UnitDiscount, string PriceHt, string PriceTTC)
        {


            try
            {
                Product product = new DbManager<Product>().Get(p => p.UserId == userId && p.ID == ProductId);
                Order order = new DbManager<Order>().Get(p => p.UserId == userId && p.ID == OrderId);
                if (product == null && order == null)
                {
                    return new { success = false, _acts = new { title = "Une erreur s'est produite!" } };
                }
                else
                {
                    if (product.Stock < Quantity)
                    {
                        return new { success = false, _acts = new { title = "La quantité demandée est supérieure au stock disponible!" } };
                    }
                    OrderLine oLine = new OrderLine { ID = System.Guid.NewGuid().ToString(), UserId = userId, ProductId = ProductId, TVA = product.TVA, UnitPrice = product.PriceHT, AmountTotalTTc = double.Parse(PriceTTC), Quantity = Quantity, AmountTotalHT = double.Parse(PriceHt), Discount = double.Parse(UnitDiscount) };
                    if (order.Lines == null)
                        order.Lines = new List<OrderLine>();
                    new DbManager<OrderLine>().AddOrderLine(order, oLine);
                    order.AmountTotalHT += double.Parse(PriceHt);
                    order.AmountTotal += double.Parse(PriceTTC);
                    new DbManager<Order>().Update(order);
                    product.Stock -= Quantity;
                    new DbManager<Product>().Update(product);


                    return new { success = true, _acts = new { title = "Produit ajouté", oID = oLine.ID, newAmountTotal = order.AmountTotal } };
                }
            }
            catch (Exception e)
            {
                return new { success = false, _acts = new { title = e.Message } };
            }
        }
        public List<Order> GetAllOrder(string userId)
        {
            var list = new List<Order>();
            using (var context = new MyEntitiesContext())
            {
                list = context.Order.Where(o=>o.UserId==userId).Include(p => p.Lines).ToList();
            }

            return list;
        }
        public List<Order> GetAllOrderInTimeRange(string userId,DateTime from, DateTime to)
        {
            var list = new List<Order>();
            using (var context = new MyEntitiesContext())
            {
                list = context.Order.Where(o => o.UserId == userId && from <= o.DateCreation && o.DateCreation <= to).Include(p => p.Lines).ToList();
            }

            return list;
        }

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
        public Object UpdateDiscount(string userId, string OrderId, int DiscountTVA, string DiscountHT, string DiscountTTC)
        {
            try
            {


                Order order = new DbManager<Order>().Get(p => p.UserId == userId && p.ID == OrderId);
                if (order == null)
                {
                    return new { success = false, _acts = new { title = "Une erreur s'est produite!" } };
                }
                double previousTTC = order.DiscountTTC;
                double previousHT = order.DiscountHT;
                order.DiscountTTC = double.Parse(DiscountTTC);
                order.DiscountHT = double.Parse(DiscountHT);
                order.DiscountTVA = DiscountTVA;
                order.AmountTotal = order.AmountTotal + previousTTC - order.DiscountTTC;
                order.AmountTotalHT = order.AmountTotalHT + previousHT - order.DiscountHT;
                new DbManager<Order>().Update(order);
                return new { success = true, _acts = new { title = "Modification effectuée" } };
            }
            catch (Exception e)
            {
                return new { success = false, _acts = new { title = e.Message } };
            }
        }
    }
    
}
