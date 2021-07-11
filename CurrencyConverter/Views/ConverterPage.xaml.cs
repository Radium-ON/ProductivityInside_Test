using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using CurrencyConverter.ViewModels;

using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter.Views
{
    public sealed partial class ConverterPage : Page
    {
        public ConverterViewModel ViewModel => (ConverterViewModel)DataContext;

        public ConverterPage()
        {
            InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (titleBar != null)
            {
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.DarkSlateGray;
                Window.Current.SetTitleBar(app_title_bar);
            }
            DataContext = App.Current.Services.GetService<ConverterViewModel>();
        }
    }
}
