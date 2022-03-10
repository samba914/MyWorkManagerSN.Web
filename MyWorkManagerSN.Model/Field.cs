using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkManagerSN.Model
{
    public class Field : mwmObject
    {
        public string Label { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
