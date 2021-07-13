namespace CurrencyConverter.Core.Models
{
    public class ExchangePair
    {
        public Currency SourceCurrency { get; }
        public Currency TargetCurrency { get; }
        public double RatioSource { get; }
        public double RatioTarget { get; }

        public ExchangePair(Currency sourceCurrency, Currency targetCurrency, double ratioSource, double ratioTarget)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            RatioSource = ratioSource;
            RatioTarget = ratioTarget;
        }

        #region Overrides of Object

        public override string ToString()
        {
            return $"{SourceCurrency.Name} - {TargetCurrency.Name}";
        }

        #endregion
    }
}