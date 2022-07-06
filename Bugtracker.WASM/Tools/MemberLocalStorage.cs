using Microsoft.JSInterop;

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
            await _JsRuntime.InvokeVoidAsync("localStorage.setItem","token", jwtoken);
        }
    }
}
