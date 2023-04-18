using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.ViewModels
{
    class OrderDetailsViewModel : BaseViewModel
    {
        public ObservableCollection<OrderDetailsProductModel> productList;
        private OrderDetailsRepository _repo = new OrderDetailsRepository();

        public OrderDetailsViewModel(int currentOrderID) { 
            productList = _repo.getAllProductOfOrder(currentOrderID);
        }

        public void removeProductFromList(int productId, int orderId)
        {
            int i = 0;

            for(; i < productList.Count; i++) {
                if (productList[i].ProductID == productId) { 
                    productList.RemoveAt(i); 
                    break; 
                }
            }

            _repo.removeProductFromOrder(productId, orderId);
        }
    }
}
