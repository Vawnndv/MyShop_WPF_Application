using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.ViewModels
{
    class AddNewOrderViewModel
    {
        private OrderDetailsRepository _repoDetail;
        private NewOrderRepository _repoNew;

        public AddNewOrderViewModel() {
            _repoDetail = new OrderDetailsRepository();
            _repoNew = new NewOrderRepository();
        }

        public List<Status> getStatusList()
        {
            return _repoDetail.getOrderStatusList();
        }

        public List<PromotionModel> getPromotionList()
        {
            return _repoDetail.getPromotionListFromDB();
        }

        public void addNewOrder(OrderModel newOrder)
        {
            _repoNew.addNewOrder(newOrder);
        }

        public Boolean addCustomer(CustomerModel newCustomer) {
            return _repoNew.addNewCustomer(newCustomer);
        }

        public void updateCustomer(CustomerModel newCustomer)
        {
            _repoNew.updateCustomer(newCustomer);
        }
    }
}
