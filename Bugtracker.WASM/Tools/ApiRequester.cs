using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Tools
{
    public class ApiRequester : IApiRequester
    {

        private HttpClient Http { get; set; }
        private string _baseUri = "https://localhost:7051/api/";
        public ApiRequester(HttpClient http)
        {
            Http = http;
        }
        public async Task<T> Get<T>(string uriEnd, string token)
        {
            string completeUri = _baseUri + uriEnd;
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Http.GetFromJsonAsync<T>(completeUri);
        }
        public async Task<HttpResponseMessage> Post<T>(T item, string uriEnd, string token = "")
        {
            string completeUri = _baseUri + uriEnd;
            if (!string.IsNullOrEmpty(token))
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Http.PostAsJsonAsync(completeUri, item);
        }
        public async Task<HttpResponseMessage> Put<T>(T item, string uriEnd, string token)
        {
            string completeUri = _baseUri + uriEnd;
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Http.PutAsJsonAsync(completeUri, item);
        }
        public async Task Delete(string uriEnd, string token)
        {
            string completeUri = _baseUri + uriEnd;
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            await Http.DeleteAsync(completeUri);
        }
    }
}
