using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyConverter;
using CurrencyConverter.Core.Helpers;
using CurrencyConverter.Core.Models;
using CurrencyConverter.Core.Services;
using CurrencyConverter.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Test
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public async Task GetRatioInfoTest()
        {
            var currSourceEur = new Currency("R01239",
                978,
                "EUR",
                1,
                "Евро",
                88.1397,
                88.7755);
            var currTargetUsd = new Currency("R01235",
                840,
                "USD",
                1,
                "Доллар США",
                74.4675,
                75.1952);

            var exchange = new ExchangeService();

            var pair = await exchange.GetRatio(currSourceEur, currTargetUsd);

            var vm = new ConverterViewModel(new JsonParcer(), new HttpDataService("https://www.cbr-xml-daily.ru/daily_json.js"), new ExchangeService());
            vm.CurrencySource = currSourceEur;
            vm.CurrencyTarget = currTargetUsd;
            await vm.CurrencySelectionChanged();
            vm.SourceFocusCommand.Execute(null);
            var sourceRatioInfo = vm.RatioInfo;
            vm.TargetFocusCommand.Execute(null);
            var targetRatioInfo = vm.RatioInfo;
            Assert.AreEqual("1 EUR = 1,1836 USD", sourceRatioInfo);
            Assert.AreEqual("1 USD = 0,8449 EUR", targetRatioInfo);

        }
    }
}
