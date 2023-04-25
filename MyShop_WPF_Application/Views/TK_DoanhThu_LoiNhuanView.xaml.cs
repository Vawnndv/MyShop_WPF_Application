using InteractiveDataDisplay.WPF;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TK_DoanhThu_LoiNhuanView.xaml
    /// </summary>
    public partial class TK_DoanhThu_LoiNhuanView : UserControl
    {
        int[] listYear = { 2024, 2023, 2022, 2021, 2020, 2019 };
        int[] listMonth = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int[] listWeek = { 1, 2, 3, 4 };
        TK_DoanhThu_LoiNhuanViewModel _viewModel = new TK_DoanhThu_LoiNhuanViewModel();

        LineGraph line1 = new LineGraph();
        LineGraph line2 = new LineGraph();
        LineGraph line3 = new LineGraph();
        private void refresh(DateTime start, DateTime end)
        {
            Lines.Children.Clear();
            var tempTuple = _viewModel.getListRevenueAndProfit(start, end);
            if (tempTuple == null)
                return;
            // Đường doanh thu
            Lines.Children.Add(line1);
            line1.Stroke = new SolidColorBrush(Colors.SaddleBrown);
            line1.Description = "Doanh thu";
            line1.StrokeThickness = 2;

            // Đường vốn
            Lines.Children.Add(line2);
            line2.Stroke = new SolidColorBrush(Colors.Red);
            line2.Description = "Vốn";
            line2.StrokeThickness = 2;

            // Đường vốn
            Lines.Children.Add(line3);
            line3.Stroke = new SolidColorBrush(Colors.Green);
            line3.Description = "Lợi nhuận";
            line3.StrokeThickness = 2;

            line1.Plot(tempTuple.Item1, tempTuple.Item2);
            line2.Plot(tempTuple.Item1, tempTuple.Item3);
            line3.Plot(tempTuple.Item1, tempTuple.Item4);
        }

        private void updateDuration(DateTime start, DateTime end)
        {
            string s = start.ToShortDateString();
            string e = end.ToShortDateString();

            txtDuration.Content = "Từ ngày " + s + " đến " + e;
        }
        public TK_DoanhThu_LoiNhuanView()
        {
            InitializeComponent();

            chooseYear.ItemsSource = listYear;
            chooseMonth.ItemsSource = listMonth;
            chooseWeek.ItemsSource = listWeek;

            DateTime _start = new DateTime(2000, 1, 1);
            DateTime _end = DateTime.Now;
            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Parse(startDatePicker.Text + " 12:00");
            DateTime end = DateTime.Parse(endDatePicker.Text + " 12:00");

            updateDuration(start, end);
            refresh(start, end);
        }

        private void chooseYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime start = new DateTime(listYear[chooseYear.SelectedIndex], 1, 1);
            DateTime end = new DateTime(listYear[chooseYear.SelectedIndex], 12, 31);

            updateDuration(start, end);
            refresh(start, end);
        }

        private void chooseMonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime end = DateTime.Now;
            DateTime start = end.AddMonths(-listMonth[chooseMonth.SelectedIndex]);

            updateDuration(start, end);
            refresh(start, end);
        }

        private void chooseWeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime end = DateTime.Now;
            DateTime start = end.AddDays(-listWeek[chooseWeek.SelectedIndex] * 7);

            updateDuration(start, end);
            refresh(start, end);
        }
    }
}
