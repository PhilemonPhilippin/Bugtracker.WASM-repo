namespace Bugtracker.WASM.Tools
{
    public interface IApiRequester
    {
        Task<T> Get<T>(string url, string token);
    }
}
