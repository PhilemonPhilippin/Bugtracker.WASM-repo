using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bugtracker.WASM.Models;

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
            //await AskTokenValidation();
            _token = await LocalStorage.GetToken();
            if (_token is null)
                _isMemberConnected = false;
            else
                _isMemberConnected = true;
        }
        //private async Task AskTokenValidation()
        //{
        //    _token = await LocalStorage.GetToken();
        //    if (_token is null)
        //        _isMemberConnected = false;
        //    else
        //    {
        //        Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        //        HttpResponseMessage response = await Http.GetAsync("https://localhost:7051/api/Member/token");
        //        if (!response.IsSuccessStatusCode)
        //            _isMemberConnected = false;
        //        else
        //        {
        //            ConnectedMemberModel connectedMember = await response.Content.ReadFromJsonAsync<ConnectedMemberModel>();
        //            await LocalStorage.SetToken(connectedMember.Token);
        //            _isMemberConnected = true;
        //        }
        //    }
        //}
    }
}
