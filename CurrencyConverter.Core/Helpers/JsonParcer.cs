using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyConverter.Core.Interfaces;
using CurrencyConverter.Core.Models;
using Newtonsoft.Json.Linq;

namespace CurrencyConverter.Core.Helpers
{
    public class JsonParcer : IJsonParse<JToken, Currency>
    {
        #region Implementation of IJsonParse<JToken,Currency>

        public Task<List<Currency>> SelectList(JToken deserializedObject)
        {
            return Task.Run(() =>
            {
                var currencyTokens = deserializedObject.Last.Last.ToList();
                return currencyTokens.Select(token => token.First.ToObject<Currency>()).ToList();
            });
        }

        #endregion
    }
}