using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.Core.Interfaces
{
    public interface IJsonParse<T, TCurrency>
    {
        Task<List<TCurrency>> SelectList(T deserializedObject);
    }
}