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

    public class QLLOAISPViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryTypeStatistic> _categoryList;
        public CategoryModel _category = null;

        private CategoryRepository _repository = new CategoryRepository();

        public QLLOAISPViewModel()
        {
            _categoryList = _repository.getCategoryWithProduct();
            _category = new CategoryModel();    
        }

        public ObservableCollection<CategoryTypeStatistic> getCategory()
        {
            return  _repository.getCategoryWithProduct();
        }

        public bool AddNewCategory(CategoryModel newCategory)
        {
            return _repository.addCategory(newCategory);
        }

        public bool EditCategory(CategoryModel editCategory)
        {
            return _repository.editCategory(editCategory);
        }

        public bool RemoveCategory(int? cId)
        {
            return _repository.removeCategory(cId);
        }
    }
}
