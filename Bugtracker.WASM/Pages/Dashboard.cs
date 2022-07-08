using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        private string _token;
        private bool _isMemberConnected;
        private bool _displayMembers;

        protected override async Task OnInitializedAsync()
        {
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            
            else
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member/token", _token);
                if (!response.IsSuccessStatusCode)
                    _isMemberConnected = false;
                else
                    _isMemberConnected = true;
            }
        //private async Task AskTokenValidation()
        //{
            //_token = await LocalStorage.GetToken();
            //if (_token is null)
            //    _isMemberConnected = false;
            //else
            //{
            //    Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //    HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member/token", _token);
            //    if (!response.IsSuccessStatusCode)
            //        _isMemberConnected = false;
            //    else
            //        _isMemberConnected = true;
            //}
        }
    }
}
