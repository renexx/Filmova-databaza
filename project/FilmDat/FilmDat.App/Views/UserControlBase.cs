using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FilmDat.App.ViewModels.Interfaces;

namespace FilmDat.App.Views
{
    public abstract class UserControlBase : UserControl
    {
        protected UserControlBase() => Loaded += OnLoaded;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IListViewModel viewModel)
            {
                viewModel.Load();
            }
        }
    }
}