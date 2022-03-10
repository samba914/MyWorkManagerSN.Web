using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    [Owned]
    public class Address 
    {
        public string? Rue { get; set; }
        public string? Ville { get; set; }
        public string? Complement { get; set; }
        public string? CodePostal { get; set; }
        public string? PaysCode { get; set; }
        
        
    }
}
