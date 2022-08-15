using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class QuoteLine : mwmObject
    {

        
        
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public double UnitPrice { get; set; }
        public int TVA { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double AmountTotalTTc { get; set; }
        public double AmountTotalHT { get; set; }
       
        //public string  OrderID { get; set; }

    }
}
