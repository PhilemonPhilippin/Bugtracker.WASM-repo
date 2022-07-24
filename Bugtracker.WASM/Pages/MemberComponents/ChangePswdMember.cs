using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using Bugtracker.WASM.Tools;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class ChangePswdMember
    {
        [Inject]
        private HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; }
        [Parameter]
        public int? MemberId { get; set; }
        private MemberChangePswdModel pswdModel = new MemberChangePswdModel();
        private MemberPostPswdModel postPswdModel = new MemberPostPswdModel();
        private bool _displayIncorrectPassword;
        private bool _displayPasswordChanged;
        private bool _isMemberConnected;
        private string _token;

        private async Task Submit()
        {
            _displayIncorrectPassword = false;
            _displayPasswordChanged = false;
            _isMemberConnected = await LocalStorage.HasToken();
            if (_isMemberConnected)
            {
                postPswdModel.IdMember = (int)MemberId;
                postPswdModel.NewPassword = pswdModel.NewPassword;
                postPswdModel.OldPassword = pswdModel.OldPassword;
                _token = await LocalStorage.GetToken();
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                using HttpResponseMessage response = await Http.PostAsJsonAsync("https://localhost:7051/api/Member/changepswd", postPswdModel);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage.Contains("Password incorrect."))
                    {
                        _displayIncorrectPassword = true;
                    }
                }
                else
                {
                    _displayPasswordChanged = true;
                    pswdModel = new MemberChangePswdModel();
                }
            }
        }
    }
}
