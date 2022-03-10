using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Order : mwmObject
    {
        public int NumOrder { get; set; }
        public ICollection<OrderLine>? Lines { get; set; }
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public string Status { get; set; }
        public double Discount { get; set; }
        public double AmountPaid { get; set; }
        public double AmountTotal { get; set; }
        public DateTime DateCreation { get; set; }
        public string? PaymentModeId { get; set; }
        public virtual PaymentMode? PaymentMode { get; set; }
        public bool IsOrderSubuscription { get; set; }
        
        



    }
}
