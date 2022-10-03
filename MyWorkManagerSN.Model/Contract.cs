using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Contract : mwmObject
    {
        public float InvoiceAmount { get; set; }
        public string ContractPeriodicityCode { get; set; }  // A,M,S,T,2M
        public DateTime DateStarted { get; set; }
        public DateTime NextInvoiceDate { get; set; }
        public bool IsActive { get; set; }
        public string InvoiceEmail { get; set; }
        public bool IsLateOnpAyment { get; set; }
        public float RemaningAmountOnInvoice { get; set; }

    }
}
