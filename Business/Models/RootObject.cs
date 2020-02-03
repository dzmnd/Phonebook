using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class RootObject
    {
        public List<Results> results { get; set; }
        public Info info { get; set; }
    }
}
