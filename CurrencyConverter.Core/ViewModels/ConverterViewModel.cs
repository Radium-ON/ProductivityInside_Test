using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Core.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Core.ViewModels
{
    public class ConverterViewModel : ObservableObject
    {
        private readonly IJsonParse<JToken, Currency> _jsonParser;
        private readonly IHttpService _httpService;
        private readonly IExchange<Currency, ExchangePair> _currencyConverter;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private List<ExchangePair> _pairs;
        private ObservableCollection<Currency> _currencies;
        private Currency _curSource;
        private Currency _curTarget;
        private ExchangePair _pair;
        private string _ratioInfo;
        private double _sourceConvertsion;
        private double _targetConversion;
        private bool _loadingEnded;
        private string _updateTimeInfo;

        public enum FieldsToSelection
        {
            Source,
            Target
        }

        public ConverterViewModel(IJsonParse<JToken, Currency> jsonParser, IHttpService httpService, IExchange<Currency, ExchangePair> currencyConverter)
        {
            _jsonParser = jsonParser;
            _httpService = httpService;
            _currencyConverter = currencyConverter;

            UpdateCommand = new RelayCommand(async () => await UpdateData());
            EnterDigitCommand = new RelayCommand<string>(EnterDigit);
            SourceFocusCommand = new RelayCommand(() =>
            {
                SelectedField = FieldsToSelection.Source;
                SetRatioInfo(SelectedField);
            });
            TargetFocusCommand = new RelayCommand(() =>
            {
                SelectedField = FieldsToSelection.Target;
                SetRatioInfo(SelectedField);
            });

            ExchangePairs = new List<ExchangePair>();
        }

        private void SetRatioInfo(FieldsToSelection field)
        {
            switch (field)
            {
                case FieldsToSelection.Source:
                    RatioInfo = $"1 {CurrentPair.SourceCurrency.CharCode} = {CurrentPair.RatioSource:#0.0000} {CurrentPair.TargetCurrency.CharCode}";
                    break;
                case FieldsToSelection.Target:
                    RatioInfo = $"1 {CurrentPair.TargetCurrency.CharCode} = {CurrentPair.RatioTarget:#0.0000} {CurrentPair.SourceCurrency.CharCode}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task InitializeProperties()
        {
            await UpdateData();
        }

        private void EnterDigit(string param)
        {
            //todo очистка builder, запись числа в поле, выбор поля вывода
            _stringBuilder.Append(param);
            var result = double.Parse(_stringBuilder.ToString());
            switch (SelectedField)
            {
                case FieldsToSelection.Source:
                    SourceConversion = result;
                    break;
                case FieldsToSelection.Target:
                    TargetConversion = result;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public FieldsToSelection SelectedField { get; set; }

        public List<ExchangePair> ExchangePairs
        {
            get => _pairs;
            set => SetProperty(ref _pairs, value);
        }

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        public Currency CurrencySource
        {
            get => _curSource;
            set => SetProperty(ref _curSource, value);
        }

        public Currency CurrencyTarget
        {
            get => _curTarget;
            set => SetProperty(ref _curTarget, value);
        }

        public ExchangePair CurrentPair
        {
            get => _pair;
            set => SetProperty(ref _pair, value);
        }

        public string RatioInfo
        {
            get => _ratioInfo;
            set => SetProperty(ref _ratioInfo, value);
        }

        public double SourceConversion
        {
            get => _sourceConvertsion;
            set => SetProperty(ref _sourceConvertsion, value);
        }

        public double TargetConversion
        {
            get => _targetConversion;
            set => SetProperty(ref _targetConversion, value);
        }

        public string UpdateTimeInfo
        {
            get => _updateTimeInfo;
            set => SetProperty(ref _updateTimeInfo, value);
        }

        public bool LoadingEnded
        {
            get => _loadingEnded;
            set => SetProperty(ref _loadingEnded, value);
        }

        public async Task CurrencySelectionChanged()
        {
            await SetPair();
        }

        private async Task SetPair()
        {
            var pair = ExchangePairs.FirstOrDefault(p =>
                p.TargetCurrency.Id == CurrencyTarget.Id && p.SourceCurrency.Id == CurrencySource.Id);
            if (pair != null)
            {
                CurrentPair = pair;
            }
            else if (CurrencySource != null && CurrencyTarget != null)
            {
                CurrentPair = await _currencyConverter.GetRatio(CurrencySource, CurrencyTarget);
                ExchangePairs.Add(CurrentPair);
            }
        }

        public IRelayCommand UpdateCommand { get; }
        public IRelayCommand ClearInputCommand { get; }
        public IRelayCommand DeleteDigitCommand { get; }
        public IRelayCommand<string> EnterDigitCommand { get; }
        public IRelayCommand SourceFocusCommand { get; }
        public IRelayCommand TargetFocusCommand { get; }

        private async Task UpdateData()
        {
            LoadingEnded = false;
            ExchangePairs?.Clear();
            var tokens = await _httpService.GetAsync<JToken>("https://www.cbr-xml-daily.ru/daily_json.js");
            Currencies = new ObservableCollection<Currency>(await _jsonParser.SelectList(tokens));
            await Task.Delay(1000);
            UpdateTimeInfo = $"Обновлено {DateTime.Now:dd.MM.yyyy hh:mm}";
            LoadingEnded = true;
        }
    }
}
