using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Category : mwmObject
    {
        public string Code { get; set; }
        public string Label { get; set; }
        public virtual List<Product> Products {get;set;}
    }
}
