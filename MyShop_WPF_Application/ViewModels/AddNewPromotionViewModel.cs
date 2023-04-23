using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.ViewModels
{
    class AddNewPromotionViewModel
    {
        private AddNewPromotionRepository _repo;

        public AddNewPromotionViewModel()
        {
            _repo = new AddNewPromotionRepository();
        }

        public void addNewPromo(PromotionModel newPromo)
        {
            _repo.addNewPromo(newPromo);
        }
    }
}
