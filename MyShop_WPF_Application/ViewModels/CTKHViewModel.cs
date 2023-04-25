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
    class CTKHViewModel : BaseViewModel
    {
        public CustomerModel _customer;
        public CustomerModel _customerRestore;


        private CustomerRepository _repository = new CustomerRepository();

        public CTKHViewModel(string? tel)
        {
            _customer = _repository.getCustomerWithPhone(tel);
            _customerRestore = _repository.getCustomerWithPhone(tel);

        }

        public bool EditCustomer(CustomerModel customer)
        {
            return _repository.editProduct(customer);
        }

        public bool RemoveCustomer(string? phone)
        {
            return _repository.removeCustomer(phone);

        }
    }
}
