﻿using Bugtracker.WASM.Models;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class EditMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        [Inject]
        private IMemberLocalStorage _LocalStorage { get; set; }
        [Parameter]
        public int MemberEditId { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public EventCallback OnConfirm { get; set; }
        private string _Token { get; set; }
        private MemberEditModel _MemberEditModel { get; set; } = new MemberEditModel();
        private bool _isPseudoTaken = false;
        private bool _isEmailTaken = false;

        protected override async Task OnInitializedAsync()
        {
            _Token = await _LocalStorage.GetToken();
            if (_Token is not null)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);

                HttpResponseMessage response = await Http.GetAsync($"https://localhost:7051/api/Member/{MemberEditId}");
                if (response.IsSuccessStatusCode)
                {
                    MemberModel memberModel = await response.Content.ReadFromJsonAsync<MemberModel>();
                    _MemberEditModel.IdMember = memberModel.IdMember;
                    _MemberEditModel.Pseudo = memberModel.Pseudo;
                    _MemberEditModel.Email = memberModel.Email;
                    _MemberEditModel.Password = memberModel.PswdHash;
                    _MemberEditModel.Firstname = memberModel.Firstname;
                    _MemberEditModel.Lastname = memberModel.Lastname;
                }
            }

           
        }
        private async Task SubmitEdit()
        {
            _isPseudoTaken = false;
            _isEmailTaken = false;
            MemberModel memberModel = new MemberModel()
            {
                IdMember = _MemberEditModel.IdMember,
                Pseudo = _MemberEditModel.Pseudo,
                Email = _MemberEditModel.Email,
                PswdHash = _MemberEditModel.Password,
                Firstname = _MemberEditModel.Firstname,
                Lastname = _MemberEditModel.Lastname
            };
            _Token = await _LocalStorage.GetToken();
            if (_Token is not null)
            {
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _Token);

                HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{MemberEditId}", memberModel);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    if (errorMessage.Contains("Pseudo and Email already exist."))
                    {
                        _isPseudoTaken = true;
                        _isEmailTaken = true;
                    }
                    else if (errorMessage.Contains("Pseudo already exists."))
                        _isPseudoTaken = true;
                    else if (errorMessage.Contains("Email already exists."))
                        _isEmailTaken = true;
                }
                else
                    await OnConfirm.InvokeAsync();
            }
                
            
        }
    }
}
