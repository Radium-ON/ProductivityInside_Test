using System.Threading.Tasks;
using CurrencyConverter.Core.Models;

namespace CurrencyConverter.Core.Interfaces
{
    public interface IExchange<in TCurrency,TPair>
    {
        Task<TPair> GetRatio(TCurrency source, TCurrency target);
        Task<double> CalcAmount(TPair pair, TCurrency curr, double value);
    }
}