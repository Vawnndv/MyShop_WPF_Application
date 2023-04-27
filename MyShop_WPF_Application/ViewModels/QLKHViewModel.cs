using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.ViewModels
{

    class QLKHViewModel : BaseViewModel
    {
        public CustomerModel _customer;
        public ObservableCollection<CustomerModel> _customerList;
        private CustomerRepository _repository = new CustomerRepository();


        public QLKHViewModel()
        {
            _customerList = _repository.getAllCustomer();
            _customer = new CustomerModel();
        }

        public bool getCustomerPhone(string? tel)
        {
            Debug.WriteLine("dộ dài:  " + _repository.getAllCustomer().Where(x => x.phone.Contains(tel)).Count());
            return _repository.getAllCustomer().Where(x => x.phone.Contains(tel)).Count()  == 1;
        }

        public ObservableCollection<CustomerModel> updateCusstomerList()
        {
            _customerList = _repository.getAllCustomer();
            return _customerList;
        }

        public bool AddCustomer(CustomerModel customer)
        {
            return _repository.addCustomer(customer);
        }

        public bool RemoveCustomer(string? phone)
        {
            return _repository.removeCustomer(phone);

        }
    } 
}
