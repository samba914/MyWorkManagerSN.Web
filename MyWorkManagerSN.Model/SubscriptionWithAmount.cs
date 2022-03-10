using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class SubscriptionWithAmount: mwmObject
    {
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public double Amount { get; set; }
    }
}
