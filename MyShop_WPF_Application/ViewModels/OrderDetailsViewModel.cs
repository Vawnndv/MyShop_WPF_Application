using Microsoft.EntityFrameworkCore.Storage;
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
            
            return true;
        }

        public double calculateTotalMoney(double promoPercentage)
        {
            double total = 0;

            for(int i = 0; i < productList.Count; ++i)
            {
                total += productList[i].orderQuantity * productList[i].ProductPrice;
            }

            total = total * (100 - promoPercentage) / 100;

            return total;
        }

        public CustomerModel getCustormerFromDB(int orderID)
        {
            return _repo.getCustomer(_repo.getCustomerPhone(orderID));
        }

        public DateTime getDateFromDB(int orderID)
        {
            return _repo.getOrderDate(orderID);
        }

        public List<Status> orderStatusList()
        {
            return _repo.getOrderStatusList();
        }

        public int getOrderStatusKey(int orderId)
        {
            return _repo.getOrderStatus(orderId);
        }

        public void updateStatus(int orderId, int status)
        {
            _repo.updateOrderStatus(orderId, status);
        }

        public void updateInfo(string phone, string newVal, string type)
        {
            _repo.updateCustomerInfo(phone, newVal, type);
        }

        public void updateDate(int orderID, string date)
        {
            _repo.updateCreateDate(orderID, date);
        }

        public List<PromotionModel> getPromotionList()
        {
            return _repo.getPromotionListFromDB();
        }

        public int? getPromotionID(int orderID)
        {
            return _repo.getPromotionID(orderID);
        }

        public void updatePromo(int orderID, int? promotionID)
        {
            // No promotion
            if (promotionID == 0)
                promotionID = null;

            _repo.updatePromotionInOrderDetails(orderID, promotionID);
        }

        public void updateTotal(int orderID, double newTotal)
        {
            _repo.updateTotalInDB(orderID, newTotal);
        }
    }
}
