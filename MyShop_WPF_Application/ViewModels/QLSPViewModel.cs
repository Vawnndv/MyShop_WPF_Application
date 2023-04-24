using MyShop_WPF_Application.Model;
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
    class QLSPViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> _productList;
        public ObservableCollection<CategoryModel> _categoryList;
        public ProductModel _product;
        private ProductRepository _repository = new ProductRepository();
        private CategoryRepository _repository2 = new CategoryRepository();

        public QLSPViewModel()
        {
            // query and get all orders
            _productList = _repository.getAllProduct();
            _categoryList = _repository2.getAllCategory();
            _product = new ProductModel();
        }

        public ObservableCollection<ProductModel> getProductList()
        {
            _productList = _repository.getAllProduct();
            return _productList;
        }

        public bool AddNewProduct(ProductModel product)
        {
            return _repository.addProduct(product);
        }
    }
}
