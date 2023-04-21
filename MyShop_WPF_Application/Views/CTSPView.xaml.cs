using Microsoft.Win32;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for CTSPView.xaml
    /// </summary>
    public partial class CTSPView : UserControl
    {
        FileInfo _selectImage = null;
        CTSPViewModel _viewModel = null;
        private int? productID;


        public CTSPView(int? pID)
        {
            InitializeComponent();

            productID = pID;
            _viewModel = new CTSPViewModel(pID);
            base.DataContext = _viewModel;
        }

        private void ListBill_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnAddBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CategoryID = int.Parse(editProductCategory.Text);
            _viewModel.ProductName = editProductName.Text;
            _viewModel.ProductPrice = int.Parse(editProductPrice.Text);
            _viewModel.ProductPriceOriginal = int.Parse(editProductPriceOriginal.Text);
            _viewModel.ProductQuantity = int.Parse(editProductQuantity.Text);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                string newPath = $"{folder}img/{_selectImage.Name}";

                if (!File.Exists(newPath))
                {
                    File.Copy(_selectImage.FullName, newPath);
                }
                _viewModel.ProductAvatar = $"img/{_selectImage.Name}";
            }
            catch (Exception ex)
            {

            }
        }
    
        private void BtnStatistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnAddImageProduct_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                _selectImage = new FileInfo(screen.FileName);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(screen.FileName, UriKind.Absolute);
                bitmap.EndInit();
                editProductAvatar.Source = bitmap;
            }
        }
    }
}
