using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    [Owned]
    public class AccountOptions
    {
        public bool ActiveSubWithAmount { get; set; }   
    }
}
