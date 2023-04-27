using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop_WPF_Application.ViewModels
{
    class TK_DoanhThu_LoiNhuanViewModel : BaseViewModel
    {
        public ObservableCollection<RevenueProfitStatisticModel> _list;

        private RevenueProfitStatisticRepositories _repository = new RevenueProfitStatisticRepositories();

        public TK_DoanhThu_LoiNhuanViewModel()
        {
            // TODO
        }

        public ObservableCollection<RevenueProfitStatisticModel> getAllRevenueAndProfit(DateTime start, DateTime end)
        {
            return _repository.getAllRevenueAndProfit(start, end);
        }

        public Tuple<double[], double[], double[], double[]>? getListRevenueAndProfit (DateTime start, DateTime end)
        {
            ObservableCollection<RevenueProfitStatisticModel> temp = getAllRevenueAndProfit(start, end);
            if (temp.Count == 0)
            {
                return null;
            }
            int numOfDays = (end - start).Days;

            double[] days = new double[numOfDays];
            double[] revenue = new double[numOfDays];
            double[] profit = new double[numOfDays];
            double[] capital = new double[numOfDays];

            int index = 0;
            DateTime curDate = start;
            for (int i = 0; i < numOfDays; i++)
            {
                days[i] = i + 1;
                if (index >= temp.Count)
                    continue;

                if (curDate == temp[index].date)
                {
                    revenue[i] = temp[index].revenue;
                    profit[i] = temp[index].profit;
                    capital[i] = temp[index].capital;

                    index++;
                } else
                {
                    revenue[i] = 0;
                    profit[i] = 0;
                    capital[i] = 0;
                }
                curDate = start.AddDays(i + 1);
            }
            var tempTuple = Tuple.Create(days, revenue, capital, profit);
            return tempTuple;
        }
    }
}
