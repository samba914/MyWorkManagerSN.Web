using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Quote : mwmObject
    {
        public int NumQuote { get; set; }
        public ICollection<QuoteLine>? Lines { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string Status { get; set; }
        public int DiscountTVA  { get; set; }
        public double DiscountHT { get; set; }
        public double DiscountTTC { get; set; }
        public double AmountPaid { get; set; }
        public double AmountTotalHT { get; set; }
        public double AmountTotal { get; set; }
        public DateTime DateCreation { get; set; }
        public string? Order_Id {get; set; }
 
        
        



    }
}
