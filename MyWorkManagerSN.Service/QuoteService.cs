using Microsoft.EntityFrameworkCore;
using MyWorkManagerSN.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Service
{
    public class QuoteService
    {
        public string CreateQuote(string userId, string date, string customerId)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Quote q = new Quote();
            q.Status = "En cours";
            q.CustomerId = customerId;
            q.AmountPaid = 0;
            q.AmountTotal = 0;
            q.AmountTotalHT = 0;
            q.DateCreation = dateTime;
            q.DiscountHT = 0;
            q.DiscountTTC = 0;
            q.DiscountTTC = 0;
            q.UserId = userId;

            q.Lines = new List<QuoteLine>();
            List<Quote> listQuote = new DbManager<Quote>().GetAll(obj => obj.UserId == userId);
            if (listQuote.Count > 0)
            {
                q.NumQuote = listQuote.OrderBy(x => x.NumQuote).ToList().Last().NumQuote + 1;
            }
            else
            {
                q.NumQuote = 1;
            }

            return new DbManager<Quote>().Add(q);

        }
        public Object AddQuoteLine(string userId, string QuoteId, string ProductId, int Quantity, string UnitDiscount, string PriceHt, string PriceTTC)
        {


            try
            {
                Product product = new DbManager<Product>().Get(p => p.UserId == userId && p.ID == ProductId);
                Quote quote = new DbManager<Quote>().Get(p => p.UserId == userId && p.ID == QuoteId);
                if (product == null && quote == null)
                {
                    return new { success = false, _acts = new { title = "Une erreur s'est produite!" } };
                }
                else
                {
                   
                    QuoteLine oLine = new QuoteLine { ID = System.Guid.NewGuid().ToString(), UserId = userId, ProductId = ProductId, TVA = product.TVA, UnitPrice = product.PriceHT, AmountTotalTTc = double.Parse(PriceTTC), Quantity = Quantity, AmountTotalHT = double.Parse(PriceHt), Discount = double.Parse(UnitDiscount) };
                    if (quote.Lines == null)
                        quote.Lines = new List<QuoteLine>();
                    new DbManager<QuoteLine>().AddQuoteLine(quote, oLine);
                    quote.AmountTotalHT += double.Parse(PriceHt);
                    quote.AmountTotal += double.Parse(PriceTTC);
                    new DbManager<Quote>().Update(quote);
 
            


                    return new { success = true, _acts = new { title = "Produit ajouté", oID = oLine.ID, newAmountTotal = quote.AmountTotal } };
                }
            }
            catch (Exception e)
            {
                return new { success = false, _acts = new { title = e.Message } };
            }
        }
        public Quote GetQuoteWithLines(Expression<Func<Quote, bool>> predicate)
        {
            var quote = new Quote();
            using (var context = new MyEntitiesContext())
            {
                quote = context.Quote
                .Where(predicate)
                .Include(p => p.Lines)
                .SingleOrDefault();
            }
            return quote;
        }
        public double UpdateQuoteLine(Quote o, QuoteLine line)
        {
            double amountTotal = 0;
            using (var context = new MyEntitiesContext())
            {
                var existingQuote = context.Quote
                .Where(p => p.ID == o.ID)
                .Include(p => p.Lines)
                .SingleOrDefault();
                context.Quote.Include("Lines").Where(i => i.ID == o.ID).FirstOrDefault();

                if (existingQuote != null)
                {
                    // Update quote
                    context.Entry(existingQuote).CurrentValues.SetValues(o);
                    foreach (var existingLine in existingQuote.Lines.ToList())
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

        public double RemoveQuoteLine(string QuoteId, QuoteLine line, string userId)
        {
            double amountTotal = 0;
            using (var context = new MyEntitiesContext())
            {
                var existingQuote = context.Quote
                .Where(p => p.ID == QuoteId)
                .Include(p => p.Lines)
                .SingleOrDefault();
                context.Quote.Include("Lines").Where(i => i.ID == QuoteId).FirstOrDefault();

                if (existingQuote != null)
                {
                    // Update order


                    // Delete lines
                    foreach (var existingLine in existingQuote.Lines.ToList())
                    {
                        if (existingLine.ID == line.ID)
                        {
                            existingQuote.AmountTotalHT -= line.AmountTotalHT;
                            existingQuote.AmountTotal -= line.AmountTotalTTc;
                            amountTotal = existingQuote.AmountTotal;
                            context.Entry(existingQuote).CurrentValues.SetValues(existingQuote);
                            context.QuoteLines.Remove(existingLine);
                        }
                    }
                }

                context.SaveChanges();
            }
            return amountTotal;
        }

    }
}
