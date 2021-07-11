namespace CurrencyConverter.Core.Models
{
    public class Currency
    {
        public string Id { get; }

        public int NumCode { get; }

        public string CharCode { get; }

        public int Nominal { get; }

        public string Name { get; }

        public double Value { get; }

        public double Previous { get; }

        public Currency(string id, int numCode, string charCode, int nominal, string name, double value, double previous)
        {
            Id = id;
            NumCode = numCode;
            CharCode = charCode;
            Nominal = nominal;
            Name = name;
            Value = value;
            Previous = previous;
        }
    }
}