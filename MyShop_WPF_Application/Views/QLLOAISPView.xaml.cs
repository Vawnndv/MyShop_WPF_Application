using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for QLLOAISPView.xaml
    /// </summary>
    public partial class QLLOAISPView : UserControl
    {
        QLLOAISPViewModel _viewModel = new QLLOAISPViewModel();
        int index = -1;

        public QLLOAISPView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel._categoryList;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var category = (CategoryTypeStatistic)button.DataContext;

            index = category.id;

            _viewModel._category.CategoryID = category.id;

            addCategoryName.Text = category.name;
            saveCategoryButton.Visibility = Visibility.Visible;
            addCategoryButton.Visibility = Visibility.Hidden;
        }

        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var category = (CategoryTypeStatistic)button.DataContext;

            index = category.id;
            if (MessageBox.Show("Bạn muốn xóa loại sản phẩm này không?",
                "Xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (index >= 0)
                {
                
                    if (category.numOfProduct > 0)
                    {
                        if (MessageBox.Show("Tồn tại sản phẩm thuộc loại sản phẩm này" + "\nXóa tất cả sản phẩm đó?",
                            "Tồn tại sản phẩm",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            var remove = _viewModel.RemoveCategory(category.id);
                            if (remove)
                            {
                                string title = "Xóa";
                                string message = "Xóa loại sản phẩm thành công";
                                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                    }

                    else
                    {
                        var remove = _viewModel.RemoveCategory(category.id);
                        if (remove)
                        {
                            string title = "Xóa";
                            string message = "Xóa loại sản phẩm thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }

            addCategoryName.Clear();
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thêm mới một loại sản phẩm ?",
                "Thêm mới",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (addCategoryName.Text.Length == 0)
                {
                    string title = "Vui lòng điền tên loại sản phẩm!";
                    string message = "kiểm tra nhập thông tin";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _viewModel._category.CategoryName = addCategoryName.Text;
                    var add = _viewModel.AddNewCategory(_viewModel._category);
                    if (add)
                    {
                        string title = "Thêm";
                        string message = "Thêm loại sản phẩm thành công";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            addCategoryName.Clear();
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void saveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hiệu chỉnh lại loại sản phẩm này?",
                "Hiệu chỉnh",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                if (addCategoryName.Text.Length == 0)
                {
                    string title = "Chưa điền tên";
                    string message = "Vui lòng điền tên loại sản phẩm";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if(index >= 0)
                    {
                    
                        _viewModel._category.CategoryName = addCategoryName.Text;

                        var edit = _viewModel.EditCategory(_viewModel._category);
                        if (edit)
                        {
                            string message = "Hiệu chỉnh";
                            string title = "Hiệu chỉnh loại sản phẩm thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);                  
                        }
                    }
                }
            }
            addCategoryName.Clear();
            saveCategoryButton.Visibility = Visibility.Hidden;
            addCategoryButton.Visibility = Visibility.Visible;
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void importProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
