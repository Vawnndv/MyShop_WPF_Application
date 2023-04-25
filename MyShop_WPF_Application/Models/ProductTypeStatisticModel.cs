using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Models
{
    class ProductTypeStatisticModel
    {
        public int id { get; set; }
        public String? name { get; set; }
        public int numOfProduct { get; set; }

        public double sumPrice { get; set; }

        public double percentage { get; set; }
    }
}
