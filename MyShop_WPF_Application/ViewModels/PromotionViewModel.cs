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
    class PromotionViewModel : BaseViewModel
    {
        public ObservableCollection<PromotionModel> promoList;
        private PromotionRepository _repoPromo;

        public PromotionViewModel()
        {
            _repoPromo = new PromotionRepository();
            promoList = _repoPromo.getAllPromotion();

        }

        public void removePromotionFromDB(int promoID, double promoPercentage)
        {
            _repoPromo.deletePromotionFromPromotionTable(promoID, promoPercentage);

            // find deleted promo in promo list and remove it from list
            for(int i = 0; i < promoList.Count; ++i)
                if (promoList[i]._promotionId == promoID)
                {
                    promoList.RemoveAt(i);
                    break;
                }
              
        }

        public void editPromotionPercentage(int promoID, double newPromoPercentage)
        {
            _repoPromo.editPromoPercentageInDB(promoID, _repoPromo.getPromoPercentage(promoID), newPromoPercentage);
        }

        public void editPromotionName(int promoID, string name) { 
            _repoPromo.editPromoNameInDB(promoID, name);
        }
    }
}
