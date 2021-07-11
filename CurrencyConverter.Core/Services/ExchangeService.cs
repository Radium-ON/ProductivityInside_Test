using System;
using System.Threading.Tasks;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Core.Models;

namespace CurrencyConverter.Core.Services
{
    public class ExchangeService : IExchange<Currency, ExchangePair>
    {
        #region Implementation of IExchange<in Currency,ExchangePair>

        public Task<ExchangePair> GetRatio(Currency source, Currency target)
        {
            return Task.Run(() =>
            {
                if (source.Id == target.Id)
                {
                    throw new ArgumentException($"Валюты идентичны. Нельзя образовать пару для {source.Name}");
                }

                var ratioSourceBase = 1 / source.Value;
                var ratioTargetBase = 1 / target.Value;
                var ratioSource = ratioTargetBase / ratioSourceBase;
                var ratioTarget = ratioSourceBase / ratioTargetBase;
                if (ratioTarget <= 0 || ratioSource <= 0)
                {
                    throw new ArithmeticException("Ошибка данных. Коэффициенты конвертации должны быть больше 0");
                }

                return new ExchangePair(source, target, ratioSource, ratioTarget);
            });
        }

        public Task<double> CalcAmount(ExchangePair pair, Currency curr, double value)
        {
            return Task.Run(() =>
            {
                var amount = 0.0;

                if (curr == pair.TargetCurrency)
                {
                    amount = pair.RatioTarget * value;
                }
                else if (curr == pair.SourceCurrency)
                {
                    amount = pair.RatioSource * value;
                }
                else
                {
                    throw new ArgumentException($"Валюта {curr.Name} не содержится в паре {pair}");
                }

                return amount;
            });
        }

        #endregion
    }
}