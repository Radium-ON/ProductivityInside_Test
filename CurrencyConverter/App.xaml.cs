using System;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CurrencyConverter.Views;

namespace CurrencyConverter
{
    public sealed partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(ConverterPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);

        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            /*services.AddSingleton<TrainingService, TrainingService>();
            services.AddSingleton<AnalyticsService, AnalyticsService>();
            services.AddSingleton<IPlayer, MediaPlayerFoundation>();
            services.AddSingleton<ISpeechToText<RecognitionResult>, SpeechService>();
            services.AddSingleton<IPrivacySettings, PrivacySettingsEnabler>();
            services.AddSingleton<ISettingsService, SettingsService>();

            services.AddSingleton<IDialogService, DialogService>(provider => new DialogService(new CustomDialogTypeLocator()));

            services.AddSingleton<GetStudentsOption, GetStudentsOption>();
            services.AddSingleton<SignUpOptions, SignUpOptions>();
            services.AddSingleton<TrainingHistoryOptions, TrainingHistoryOptions>();
            services.AddSingleton<TrainingDetailsOptions, TrainingDetailsOptions>();
            services.AddSingleton<TrainingStartOptions, TrainingStartOptions>();
            services.AddSingleton<TrainingRunOptions, TrainingRunOptions>();
            services.AddSingleton<ResultsOptions, ResultsOptions>();
            services.AddSingleton<DatabaseConnection, DatabaseConnection>();

            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<HistoryViewModel>();
            services.AddTransient<HistoryDetailsViewModel>();
            services.AddTransient<TrainingStartViewModel>();
            services.AddTransient<TrainingRunViewModel>();
            services.AddTransient<ResultsViewModel>();
            services.AddTransient<DbConnectionDialogViewModel>();*/

            return services.BuildServiceProvider();
        }
    }
}
