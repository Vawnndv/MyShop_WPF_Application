using MaterialDesignThemes.Wpf;
using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
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
        DateTime _start;
        DateTime _end;
        int _year = 0;
        int _month = 0;
        int _week = 0;

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

            _year = 0;
            _month = 0;
            _week = 0;
        }

        private void updateDuration (DateTime start, DateTime end)
        {
            string s = start.ToString("dd/MM/yyyy");
            string e = end.ToString("dd/MM/yyyy");

            txtDuration.Content = "Từ ngày " + s + " đến " + e;
        }

        public TKSPView()
        {
            InitializeComponent();

            chooseYear.ItemsSource = listYear;
            chooseMonth.ItemsSource = listMonth;
            chooseWeek.ItemsSource = listWeek;
         
            _start = new DateTime(2000, 1, 1);
            _end = DateTime.Now;
            updateDuration(_start, _end);
            refresh(_start, _end);

            Global.SaveScreen("TKSP");
        }

        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Parse(startDatePicker.Text + " 12:00");
            DateTime end = DateTime.Parse(endDatePicker.Text + " 12:00");
            _start = start;
            _end = end;

            updateDuration(start, end);
            refresh(start, end);
        }

        public static Tuple<DateTime, DateTime> GetStartAndEndDays(int year, int month, int week)
        {
            DateTime startDay;
            DateTime endDay;
            if (year > 0 && month == 0 && week == 0)
            {
                startDay = new DateTime(year, 1, 1);
                endDay = new DateTime(year, 12, 31);
            }
            else if (year == 0 && month > 0 && week == 0)
            {
                startDay = new DateTime(DateTime.Now.Year, month, 1);
                endDay = new DateTime(DateTime.Now.Year, month, 1).AddMonths(1).AddDays(-1);
            }
            else if (year == 0 && month == 0 && week > 0)
            {
                startDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                endDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays((week * 7) - 1);
            }
            else if (year > 0 && month > 0 && week == 0)
            {
                startDay = new DateTime(year, month, 1);
                endDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1);
            }
            else if (year == 0 && month > 0 && week > 0)
            {
                startDay = new DateTime(DateTime.Now.Year, month, 1);
                endDay = new DateTime(DateTime.Now.Year, month, 1).AddDays((week * 7) - 1);
            }
            else if (year > 0 && month == 0 && week > 0)
            {
                startDay = new DateTime(year, DateTime.Now.Month, 1);
                endDay = new DateTime(year, DateTime.Now.Month, 1).AddDays((week * 7) - 1);
            }
            else
            {
                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                int daysInFirstWeek = 7 - firstDayOfWeek;
                int daysInMonth = DateTime.DaysInMonth(year, month);
                int daysLeft = daysInMonth - daysInFirstWeek;
                int weekOffset = (week - 2) * 7;
                int startDayOffset = daysInFirstWeek + weekOffset;
                int endDayOffset = startDayOffset + 6;
                startDay = firstDayOfMonth.AddDays(startDayOffset);
                endDay = firstDayOfMonth.AddDays(endDayOffset <= daysInMonth ? endDayOffset : daysLeft);
            }

            return Tuple.Create(startDay, endDay);
        }

        private void chooseYearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime start = new DateTime(listYear[chooseYear.SelectedIndex], 1, 1);
            //DateTime end = new DateTime(listYear[chooseYear.SelectedIndex], 12, 31);
            if (chooseYear.SelectedIndex < 0)
                return;
            _year = listYear[chooseYear.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void chooseMonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime end = DateTime.Now;
            //DateTime start = end.AddMonths(-listMonth[chooseMonth.SelectedIndex]);
            if (chooseMonth.SelectedIndex < 0)
                return;
            _month = listMonth[chooseMonth.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void chooseWeekComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DateTime end = DateTime.Now;
            //DateTime start = end.AddDays(- listWeek[chooseWeek.SelectedIndex] * 7);
            if (chooseWeek.SelectedIndex < 0)
                return;
            _week = listWeek[chooseWeek.SelectedIndex];
            Tuple<DateTime, DateTime> date = GetStartAndEndDays(_year, _month, _week);
            _start = date.Item1;
            _end = date.Item2;

            updateDuration(_start, _end);
            refresh(_start, _end);
        }

        private void CategoryColumn_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CategoryTypeStatistic selected = first_chart.SelectedItem as CategoryTypeStatistic;
            screen.Content = new TKCTSP(_start, _end, selected.id, selected.name);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            refreshCombobox();

            _start = new DateTime(2000, 1, 1);
            _end = DateTime.Now;
            updateDuration(_start, _end);
            refresh(_start, _end);
        }
    }
}
