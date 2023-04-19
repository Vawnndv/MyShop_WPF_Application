using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            for(int i = 0; i < productList.Count; i++) {
                if (productList[i].ProductID == productId) { 
                    productList.RemoveAt(i); 
                    break; 
                }
            }

            _repo.removeProductFromOrder(productId, orderId);
        }

        public Boolean updateProductQuantity(int orderId, int productId, int quantity, int prevQuan)
        {
            // get stock product from Database
            int stock = _repo.getProductQuantity(productId);
            int newStock = stock + prevQuan - quantity; // calculate new in stock if get an amount of product base on quantity

            // not enough product in stock
            if(newStock < 0)
                return false;
            
            for (int i = 0; i < productList.Count; i++)
            {
                if (productList[i].ProductID == productId)
                {
                    productList[i].ProductQuantity = quantity;
                   
                    break;
                }
            }

            _repo.updateStockProductQuantity(productId, newStock);
            _repo.updateProductQuantityInOrderDetail(orderId, productId, quantity);
            
            MessageBox.Show(prevQuan.ToString());

            return true;
        }
    }
}
