namespace Bugtracker.WASM.Tools
{
    public interface IApiRequester
    {
        Task<T> Get<T>(string uriEnd, string token);
        Task<HttpResponseMessage> Post<T>(T item, string uriEnd, string token = "");
        Task<HttpResponseMessage> Put<T>(T item, string uriEnd, string token);
        Task Delete(string uriEnd, string token);
    }
}
