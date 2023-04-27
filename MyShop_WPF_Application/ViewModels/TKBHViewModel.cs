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
    class TKBHViewModel : BaseViewModel
    {
        private ProductRepository _repository = new ProductRepository();

        public TKBHViewModel()
        {
        }

        public ObservableCollection<ProductBestSellModel> getTop10BestSell(DateTime start, DateTime end)
        {
            return _repository.getTop10ProductBestSelling(start, end);
        }
    }
}
