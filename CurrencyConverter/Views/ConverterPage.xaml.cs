using System;

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
            DataContext = App.Current.Services.GetService<ConverterViewModel>();
        }
    }
}
