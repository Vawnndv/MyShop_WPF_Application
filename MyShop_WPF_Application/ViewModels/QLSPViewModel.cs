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

        private ProductRepository _repository = new ProductRepository();
        private CategoryRepository _repository2 = new CategoryRepository();

        private int _ProductID { get; set; }
        private int _CategoryID { get; set; }
        private string _ProductName { get; set; }
        private string _ProductAvatar { get; set; }
        private int _ProductQuantity { get; set; }
        private double _ProductPrice { get; set; }
        private double _ProductPriceOriginal { get; set; }

        public QLSPViewModel()
        {
            // query and get all orders
            _productList = _repository.getAllProduct();
            _categoryList = _repository2.getAllCategory();
        }

        public int ProductID
        {
            get { return _ProductID; }
            set
            {
                _ProductID = value;
                OnPropertyChanged("ProductID");
            }
        }

        public int CategoryID
        {
            get { return _CategoryID; }
            set
            {
                _CategoryID = value;
                OnPropertyChanged("CategoryID");
            }
        }

        public string ProductName
        {
            get { return _ProductName; }
            set
            {
                _ProductName = value;
                OnPropertyChanged("ProductName");
            }
        }

        public string ProductAvatar
        {
            get { return _ProductAvatar; }
            set
            {
                _ProductAvatar = value;
                OnPropertyChanged("ProductAvatar");
            }
        }

        public int ProductQuantity
        {
            get { return _ProductQuantity; }
            set
            {
                _ProductQuantity = value;
                OnPropertyChanged("ProductQuantity");
            }
        }

        public double ProductPrice
        {
            get { return _ProductPrice; }
            set
            {
                _ProductPrice = value;
                OnPropertyChanged("ProductPrice");
            }
        }

        public double ProductPriceOriginal
        {
            get { return _ProductPriceOriginal; }
            set
            {
                _ProductPriceOriginal = value;
                OnPropertyChanged("ProductPriceOriginal");
            }
        }
    }
}
