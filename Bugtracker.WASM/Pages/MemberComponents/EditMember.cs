using Bugtracker.WASM.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class EditMember
    {
        [Inject]
        public HttpClient Http { get; set; }
        // Faut-il initialiser le MemberTarget?
        // Ou faut-il faire une fonction async on initialize pour le MemberEditModel ?
        [Parameter]
        public int TargetId { get; set; }
        public MemberEditModel MemberEditModel { get; set; } = new MemberEditModel();
        public bool isPseudoTaken = false;
        public bool isEmailTaken = false;

        protected override async Task OnInitializedAsync()
        {
            //MemberModel memberModel = await Http.GetFromJsonAsync<MemberModel>($"https://localhost:7051/api/Member/{TargetId}");
            HttpResponseMessage response = await Http.GetAsync($"https://localhost:7051/api/Member/{TargetId}");
            if (response.IsSuccessStatusCode)
            {
                MemberModel memberModel = await response.Content.ReadFromJsonAsync<MemberModel>();
                MemberEditModel.IdMember = memberModel.IdMember;
                MemberEditModel.Pseudo = memberModel.Pseudo;
                MemberEditModel.Email = memberModel.Email;
                MemberEditModel.Password = memberModel.PswdHash;
                MemberEditModel.Firstname = memberModel.Firstname;
                MemberEditModel.Lastname = memberModel.Lastname;
            }
        }
        private async Task SubmitEdit()
        {
            isPseudoTaken = false;
            isEmailTaken = false;
            MemberModel memberModel = new MemberModel()
            {
                IdMember = MemberEditModel.IdMember,
                Pseudo = MemberEditModel.Pseudo,
                Email = MemberEditModel.Email,
                PswdHash = MemberEditModel.Password,
                Firstname = MemberEditModel.Firstname,
                Lastname = MemberEditModel.Lastname
            };
            HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{TargetId}", memberModel);

            if (!response.IsSuccessStatusCode)
            {
               
            }
            else
                Console.WriteLine("Close the Edit member component");
        }
    }
}
