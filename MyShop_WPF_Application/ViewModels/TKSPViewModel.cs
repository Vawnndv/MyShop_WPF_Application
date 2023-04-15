using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using MyShop_WPF_Application.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop_WPF_Application.ViewModels
{
    class TKSPViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryTypeStatistic> _categoryList;

        private CategoryTypeStatisticRepository _repository = new CategoryTypeStatisticRepository();

        public TKSPViewModel ()
        {
            //_categoryList = _repository.getAllCategory();
        }

        public ObservableCollection<CategoryTypeStatistic> getAllCategory (DateTime start, DateTime end)
        {
            return _repository.getAllCategory(start, end);
        }
    }
}
