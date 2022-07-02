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
        public MemberModel MemberTarget { get; set; }

        public MemberEditModel memberEdit;
        public bool isPseudoTaken = false;
        public bool isEmailTaken = false;
        public EditMember()
        {
            MemberTarget = new MemberModel();
            memberEdit = new MemberEditModel()
            {
                IdMember = MemberTarget.IdMember,
                Pseudo = MemberTarget.Pseudo,
                Password = MemberTarget.PswdHash,
                Email = MemberTarget.Email,
                Firstname = MemberTarget.Firstname,
                Lastname = MemberTarget.Lastname
            };
        }
        private async Task SubmitEdit(int id)
        {
            isPseudoTaken = false;
            isEmailTaken = false;
            // TODO : finir cette fonction et le formulaire qui va avec
            HttpResponseMessage response = await Http.PutAsJsonAsync($"https://localhost:7051/api/Member/{id}", memberEdit);

            if (!response.IsSuccessStatusCode)
            {
                int responseNumber = await response.Content.ReadFromJsonAsync<int>();
            }
            else
                Console.WriteLine("Close the Edit member component");
        }
    }
}
