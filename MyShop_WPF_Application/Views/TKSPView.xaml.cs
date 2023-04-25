using MaterialDesignThemes.Wpf;
using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for TKSPView.xaml
    /// </summary>
    public partial class TKSPView : UserControl
    {
        int[] listYear = { 2024, 2023, 2022, 2021, 2020, 2019 };
        int[] listMonth = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        int[] listWeek = { 1, 2, 3, 4};
        TKSPViewModel _viewModel = new TKSPViewModel();

        private void refresh (DateTime start, DateTime end)
        {
            ObservableCollection<CategoryTypeStatistic> viewModels = new ObservableCollection<CategoryTypeStatistic>();
            viewModels = _viewModel.getAllCategory(start, end);

            categoryChart.ItemsSource = viewModels;
            categoryPieChart.ItemsSource = viewModels;

            categoryTurnoverChart.ItemsSource = viewModels;
            categoryTurnoverPieChart.ItemsSource = viewModels;
        }

        private void refreshCombobox ()
        {
            chooseYear.SelectedIndex = -1;
            chooseMonth.SelectedIndex = -1;
            chooseWeek.SelectedIndex = -1;
        }

        private void updateDuration (DateTime start, DateTime end)
        {
            string s = start.ToShortDateString();
            string e = end.ToShortDateString ();

            txtDuration.Content = "Từ ngày " + s + " đến " + e;
        }

        public TKSPView()
        {
            InitializeComponent();

            chooseYear.ItemsSource = listYear;
            chooseMonth.ItemsSource = listMonth;
            chooseWeek.ItemsSource = listWeek;
            
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
            DateTime start = end.AddDays(- listWeek[chooseWeek.SelectedIndex] * 7);

            updateDuration(start, end);
            refresh(start, end);
        }

        private void CategoryColumn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DataContext = new MainViewModel();
        }
    }
}
