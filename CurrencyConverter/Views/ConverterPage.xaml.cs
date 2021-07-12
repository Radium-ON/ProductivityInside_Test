using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using CurrencyConverter.Core.ViewModels;
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

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var textblock = sender as TextBlock;
            if (textblock?.Name == "tb_source")
            {
                tb_source.FontWeight = FontWeights.SemiBold;
                tb_target.FontWeight = FontWeights.Light;
            }
            else
            {
                tb_target.FontWeight = FontWeights.SemiBold;
                tb_source.FontWeight = FontWeights.Light;
            }
        }
    }
}
