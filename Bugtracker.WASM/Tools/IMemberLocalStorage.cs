namespace Bugtracker.WASM.Tools
{
    public interface IMemberLocalStorage
    {

        Task SetToken(string jwtoken);
        Task<string> GetToken();
    }
}
