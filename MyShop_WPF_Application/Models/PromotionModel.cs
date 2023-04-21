using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Models
{
    class PromotionModel
    {
        public int _promotionId {  get; set; }
        public string? _promotionName { get; set; }
        public int _promotionQuantity { get; set; }
        public double _promotionPercentage { get; set; }
    }
}
