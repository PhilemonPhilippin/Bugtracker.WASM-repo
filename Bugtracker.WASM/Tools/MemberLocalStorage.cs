using Microsoft.JSInterop;
using System.Text.Json;

namespace Bugtracker.WASM.Tools
{
    public class MemberLocalStorage : IMemberLocalStorage
    {
        private IJSRuntime _JsRuntime;

        public MemberLocalStorage(IJSRuntime jsRuntime)
        {
            _JsRuntime = jsRuntime;
        }
        public async Task SetToken(string jwtoken)
        {
            await _JsRuntime.InvokeVoidAsync("localStorage.setItem", "bugtrackertoken", JsonSerializer.Serialize(jwtoken));
        }
        public async Task<string> GetToken()
        {
            string token = await _JsRuntime.InvokeAsync<string>("localStorage.getItem", "bugtrackertoken");
            if (token is null)
                return null;
            else
                return JsonSerializer.Deserialize<string>(token);
        }
        public async Task RemoveToken()
        {
            await _JsRuntime.InvokeVoidAsync("localStorage.removeItem", "bugtrackertoken");
        }
        public async Task<bool> HasToken()
        {
            string token = await GetToken();
            return (token is null) ? false : true;
        }
    }
}
