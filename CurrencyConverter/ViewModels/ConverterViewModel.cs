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

namespace CurrencyConverter.ViewModels
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
        private double _ratio;

        public ConverterViewModel(IJsonParse<JToken, Currency> jsonParser, IHttpService httpService, IExchange<Currency, ExchangePair> currencyConverter)
        {
            _jsonParser = jsonParser;
            _httpService = httpService;
            _currencyConverter = currencyConverter;

            UpdateCommand = new RelayCommand(async () => await UpdateData());
            EnterDigitCommand = new RelayCommand<string>(EnterDigit);
            GotFocusCommand = new RelayCommand<object>(ChangeFieldToInput);

            ExchangePairs = new List<ExchangePair>();
        }

        public async Task InitializeProperties()
        {
            await UpdateData();
        }

        private void ChangeFieldToInput(object obj)
        {
            var lol = obj;
        }

        private void EnterDigit(string param)
        {
            //todo очистка builder, запись числа в поле, выбор поля вывода
            _stringBuilder.Append(param);
            int.Parse(_stringBuilder.ToString());
        }

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
            set
            {
                SetProperty(ref _curSource, value);
                SetPair();
            }
        }

        public Currency CurrencyTarget
        {
            get => _curTarget;
            set
            {
                SetProperty(ref _curTarget, value);
                SetPair();
            }
        }

        public ExchangePair CurrentPair
        {
            get => _pair;
            set => SetProperty(ref _pair, value);
        }

        public double SelectedRatio
        {
            get => _ratio;
            set => SetProperty(ref _ratio, value);
        }

        private async Task SetPair()
        {
            var pair = ExchangePairs.First(p =>
                p.TargetCurrency.Id == CurrencyTarget.Id && p.SourceCurrency.Id == CurrencySource.Id);
            if (pair != null)
            {
                CurrentPair = pair;
            }
            else
            {
                CurrentPair = await _currencyConverter.GetRatio(CurrencySource, CurrencyTarget);
                ExchangePairs.Add(CurrentPair);
            }
        }

        public IRelayCommand UpdateCommand { get; }
        public IRelayCommand<string> EnterDigitCommand { get; }
        public IRelayCommand<object> GotFocusCommand { get; }

        private async Task UpdateData()
        {
            ExchangePairs?.Clear();
            var tokens = await _httpService.GetAsync<JToken>("https://www.cbr-xml-daily.ru/daily_json.js");
            Currencies = new ObservableCollection<Currency>(await _jsonParser.SelectList(tokens));
        }
    }
}
