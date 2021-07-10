using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CurrencyConverter.Core.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string uri, string accessToken = null, bool forceRefresh = false);

        Task<bool> PostAsync<T>(string uri, T item);

        Task<bool> PostAsJsonAsync<T>(string uri, T item);

        Task<bool> PutAsync<T>(string uri, T item);

        Task<bool> PutAsJsonAsync<T>(string uri, T item);

        Task<bool> DeleteAsync(string uri);

    }
}