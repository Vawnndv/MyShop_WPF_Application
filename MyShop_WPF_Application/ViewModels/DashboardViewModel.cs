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
    class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> _top5ProductSoldList;
        public ProductRepository _repository = new ProductRepository();

        public int _quantityProductAvailable = 0;
        public int _quantityNewPurchaseInWeek = 0;
        public int _quantityNewPurchaseInMonth = 0;

        public DashboardViewModel()
        {   
            //Calculate quanlity product available 
            _quantityProductAvailable = _repository.getNumOfProductsAvailable();


            //Calculate new purchase in week
            DayOfWeek wk = DateTime.Today.DayOfWeek;
            int day = (int)wk;
            
            DateTime currentDay = DateTime.Now;
            DateTime startDay;

            if(day == 0)
            {
                startDay = currentDay.AddDays(-6);
            }
            else
            {
                startDay = currentDay.AddDays(-day + 1);
            }

            _quantityNewPurchaseInWeek = _repository.getNumOfPurchaseSold(startDay, currentDay);


            //Calculate new purchase in month
            DateTime date = DateTime.Now;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            _quantityNewPurchaseInMonth = _repository.getNumOfPurchaseSold(firstDayOfMonth, lastDayOfMonth);

            //Top 5 products sold  
            _top5ProductSoldList = _repository.getTop5Product();
        }

        private int _ProductID { get; set; }
        private int _CategoryID { get; set; }
        private string _ProductName { get; set; }
        private string _ProductAvatar { get; set; }
        private int _ProductQuantity { get; set; }
        private double _ProductPrice { get; set; }
        private double _ProductPriceOriginal { get; set; }

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
