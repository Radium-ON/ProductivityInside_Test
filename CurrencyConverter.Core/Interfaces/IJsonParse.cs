using System.Threading.Tasks;

namespace CurrencyConverter.Core.Interfaces
{
    public interface IJsonParse<T>
    {
        Task<T> SelectRoot(T deserializedObject);
        Task<T> SelectList(T dedeserializedObject);
    }
}