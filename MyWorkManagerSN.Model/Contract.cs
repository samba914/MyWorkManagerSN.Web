using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Contract : mwmObject
    {
        float InvoiceAmount { get; set; }
        string ContractPeriodicityCode { get; set; }  // A,M,S,T,2M
        DateTime DateStarted { get; set; }
        DateTime NextInvoiceDate { get; set; }
        string InvoiceEmail { get; set; }
        bool IsLateOnpAyment { get; set; }
        float RemaningAmountOnInvoice { get; set; }

    }
}
