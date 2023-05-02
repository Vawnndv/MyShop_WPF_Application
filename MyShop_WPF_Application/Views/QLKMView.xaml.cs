using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.UserControls;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for QLKMView.xaml
    /// </summary>
    public partial class QLKMView : UserControl
    {
        PromotionViewModel _viewModel;
        int _currentPage = 1;
        int _itemPerPage = 10;
        int _totalPage = 0;

        // constructor
        public QLKMView()
        {
            InitializeComponent();

            _viewModel = new PromotionViewModel();

            for (int i = 0; i < Dashboard.menuBTN.Children.Count; i++)
            {
                if (Dashboard.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            for (int i = 0; i < Dashboard.subMenuBTN.Children.Count; i++)
            {
                if (Dashboard.subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = Dashboard.subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }

            Global.SaveScreen("QLKM");
        }

        // Make sure the input are all numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addNewPromoButton_Click(object sender, RoutedEventArgs e)
        {
            screen.Content = new AddNewPromotionView();

            //_viewModel = new PromotionViewModel();
            //updatePage(_currentPage);
        }

        // delete a promotion code base on ID get from list.item
        private void deletePromotionButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            // get the promo item base on
            // the button clicked of that item on list view
            PromotionModel promo = (PromotionModel)button.DataContext;

            _viewModel.removePromotionFromDB(promo._promotionId, promo._promotionPercentage);

            lst.ClearValue(ItemsControl.ItemsSourceProperty);

            updatePage(_currentPage);
        }

        // adjust the number of item display
        // on the list in each page
        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _itemPerPage = Int16.Parse(displayRowPerPageTextBox.Text);

                updatePage(_currentPage);
            }
            catch { }
        }

        // get to next page
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if(_currentPage < _totalPage)
            {
                _currentPage++;
                updatePage(_currentPage);
            }
        }

        // get to previous page
        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if(_currentPage > 1)
            {
                _currentPage--;
                updatePage(_currentPage);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updatePage(1);

            displayRowPerPageTextBox.Text = _itemPerPage.ToString();
        }

        // update paging system
        // update current page + total page
        // change list item source base on current page + num of item per page
        private void updatePage(int page)
        {
            int listSize = _viewModel.promoList.Count;

            _currentPage = page;

            // update total page
            _totalPage = listSize / _itemPerPage + ((listSize % _itemPerPage) == 0 ? 0 : 1);

            // update list item source base on current page + item per page
            lst.ItemsSource = _viewModel.promoList.Skip((_currentPage - 1) * _itemPerPage).Take(_itemPerPage);

            pageCountLabel.Content = $"{_currentPage}/{_totalPage}";
        }

        private void editPromotionNameButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            PromotionModel selectedPromo = (PromotionModel)button.DataContext;

            if(selectedPromo._promotionName == null || selectedPromo._promotionName == "")
            {
                MessageBox.Show("Tên loại khuyến mãi không được để trống");
                return;
            }

            _viewModel.editPromotionName(selectedPromo._promotionId, selectedPromo._promotionName);
        }

        private void editPromotionPercentageButton_Click(object sender, RoutedEventArgs e)
        {
            // get current selected promo from listview
            Button button = (Button)sender;
            PromotionModel selectedPromo = (PromotionModel)button.DataContext;

            if (selectedPromo._promotionPercentage.ToString() == null || 
                selectedPromo._promotionPercentage.ToString() == "" || 
                selectedPromo._promotionPercentage < 0)
            {
                MessageBox.Show("Giá trị không hợp lệ, vui lòng kiểm tra lại");
                return; 
            }

            if (selectedPromo._promotionPercentage > 100)
            {
                MessageBox.Show("Khuyến mãi không được lớn hơn 100%");
                return;
            }

            double newPercentage = selectedPromo._promotionPercentage;

            _viewModel.editPromotionPercentage(selectedPromo._promotionId, newPercentage);

            
            MessageBox.Show("Chỉnh sửa giá trị phần trăm thành công");
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
