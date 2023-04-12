﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MyShop_WPF_Application.ViewModels;

namespace MyShop_WPF_Application.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;
        Window creatingForm;
        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public Window setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        public void Execute(object parameter)
        {

            if (parameter.ToString() == "QLSP")
            {
                viewModel.SelectedViewModel = new QLSPViewModel();
                Console.WriteLine(parameter.ToString());
            }
            else if (parameter.ToString() == "TMSP")
            {
                viewModel.SelectedViewModel = new TMSPViewModel();
            }
            else if (parameter.ToString() == "dang_xuat")
            {
                Window myWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.Name == "dashboard");

                // Kiểm tra nếu cửa sổ tồn tại và đang được hiển thị
                if (myWindow != null && myWindow.IsVisible)
                {
                    // Tắt cửa sổ
                    myWindow.Close();
                }
            }
        }
    }
}