using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public QLDHViewModel()
        {
            _orderList = _repository.getAllOrder();
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
    }
}
