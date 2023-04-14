using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Repositories;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop_WPF_Application.ViewModels
{
    class QLDHViewModel : BaseViewModel
    {
        public ObservableCollection<OrderModel> _orderList;

        private OrderRepository _repository = new OrderRepository();

        private int _OrderID { get; set; }
        private float _OrderTotal { get; set; }
        private string? _OrderDate { get; set; }
        private int _OrderStatus { get; set; }
        private int _CustomerPhone { get; set; }
        private int _PromotionID { get; set; }  
        private string _OrderStatusDisplayText { get; set; }


        public QLDHViewModel()
        {
            // query and get all orders
            _orderList = _repository.getAllOrder();
        }

        public int PromotionID
        {
            get { return _PromotionID; }
            set
            {
                _PromotionID = value;
                OnPropertyChanged("PromotionID");
            }
        }

        public int OrderID { 
            get { return _OrderID; } 
            set { 
                _OrderID = value;
                OnPropertyChanged("OrderID");

            }
        }
        public float OrderTotal
        {
            get { return _OrderTotal; }
            set
            {
                _OrderTotal = value;
                OnPropertyChanged("OrderTotal");
            }
        }

        public string? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                _OrderDate = value;
                OnPropertyChanged("OrderDate");
            }
        }
        public int OrderStatus
        {
            get { return _OrderStatus; }
            set
            {
                _OrderStatus = value;
                OnPropertyChanged("OrderStatus");
            }
        }
        public int CustomerPhone
        {
            get { return _CustomerPhone; }
            set
            {
                _CustomerPhone = value;
                OnPropertyChanged("CustomerPhone");
            }
        }

        public string OrderStatusDisplayText
        {
            get { return _OrderStatusDisplayText; }
            set
            {
                _OrderStatusDisplayText = value;
                OnPropertyChanged("OrderStatusDisplayText");
            }
        }

        // Function
        // remove order at position i (in the list and in the Database)
        public void removeOrder(int i)
        {
            if(i >= 0 && i < _orderList.Count)
            {
                _repository.deleteOrderId(_orderList[i].OrderID);
                _orderList.RemoveAt(i);
            }
        }
    }
}
