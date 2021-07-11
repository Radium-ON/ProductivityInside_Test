using CurrencyConverter.Core.Helpers;
using System;
using System.Threading.Tasks;
using CurrencyConverter.Core.Models;
using CurrencyConverter.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task JsonTokenNotNull()
        {
            var http = new HttpDataService("https://www.cbr-xml-daily.ru/daily_json.js");
            var valuteList = await http.GetAsync<JToken>("https://www.cbr-xml-daily.ru/daily_json.js");
            Assert.IsNotNull(valuteList);
        }

        [TestMethod]
        public async Task GetRatioTest()
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
            Assert.AreEqual(currTargetUsd, pair.TargetCurrency);
            Assert.AreEqual(currSourceEur, pair.SourceCurrency);
            Assert.AreEqual(Math.Round(1.18359, 4), Math.Round(pair.RatioSource, 4));
            Assert.AreEqual(Math.Round(0.84488, 4), Math.Round(pair.RatioTarget, 4));
        }

        [TestMethod]
        public async Task GetRatioTest_AgrumentException()
        {
            var currSourceEur = new Currency("R01239",
                978,
                "EUR",
                1,
                "Евро",
                88.1397,
                88.7755);
            var currTargetEur = new Currency("R01239",
                978,
                "EUR",
                1,
                "Евро",
                88.1397,
                88.7755);

            var exchange = new ExchangeService();

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await exchange.GetRatio(currSourceEur, currTargetEur));
        }

        [TestMethod]
        public async Task GetRatioTest_Coeff_low_zero()
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
                0,
                75.1952);

            var exchange = new ExchangeService();

            await Assert.ThrowsExceptionAsync<ArithmeticException>(async () => await exchange.GetRatio(currSourceEur, currTargetUsd));
        }

        [TestMethod]
        public async Task CalcAmountTest_Curr_isnt_in_pair()
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
            var emptyCur = new Currency("", 0, "", 1, "None", 9, 7);
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await exchange.CalcAmount(pair, emptyCur, 844));
        }

        [TestMethod]
        public async Task CalcAmountTest()
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
            var amountUsdFromEur = await exchange.CalcAmount(pair, currSourceEur, 844);
            var amountEurFromUsd = await exchange.CalcAmount(pair, currTargetUsd, 1000);
            Assert.AreEqual(999, Math.Round(amountUsdFromEur));
            Assert.AreEqual(845, Math.Round(amountEurFromUsd));
        }

        [TestMethod]
        public async Task SelectListTest()
        {
            var http = new HttpDataService("https://www.cbr-xml-daily.ru/daily_json.js");
            var valuteList = await http.GetAsync<JToken>("https://www.cbr-xml-daily.ru/daily_json.js");
            var json = new JsonParcer();
            var currencyList = await json.SelectList(valuteList);
            var currSourceEur = new Currency("R01239",
                978,
                "EUR",
                1,
                "Евро",
                88.1397,
                88.7755);
            Assert.AreEqual(currSourceEur.Id, currencyList.Find(c => c.CharCode == "EUR").Id);
        }
    }
}
