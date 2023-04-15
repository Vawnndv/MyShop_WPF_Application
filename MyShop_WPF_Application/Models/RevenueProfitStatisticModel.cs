using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Models
{
    class RevenueProfitStatisticModel
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public double revenue { get; set; }

        public double profit { get; set; }

        public double capital { get; set; }
    }
}
