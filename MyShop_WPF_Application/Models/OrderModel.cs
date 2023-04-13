using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Model
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public Double OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string? CustomerPhone { get; set; }

    }
}
