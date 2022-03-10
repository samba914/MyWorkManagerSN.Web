using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Product : mwmObject
    {

        public string Code { get; set; }
        public string Label { get; set; }
        public double PriceHT { get; set;}
        public int TVA { get; set; }
        public double PriceTtc { get; set; }
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int Stock { get; set; }
        public List<Field>? MoreFields { get; set; }


    }
}
