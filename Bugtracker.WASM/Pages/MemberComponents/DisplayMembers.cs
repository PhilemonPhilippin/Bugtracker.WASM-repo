using Bugtracker.WASM.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Bugtracker.WASM.Pages.MemberComponents
{
    public partial class DisplayMembers
    {
        [Inject]
        public HttpClient Http { get; set; }
        public List<MemberVm> members { get; set; } = new List<MemberVm>();

        protected override async Task OnInitializedAsync()
        {
            members = await Http.GetFromJsonAsync<List<MemberVm>>("https://localhost:7051/api/Member");
            Console.WriteLine("On est entré dans la consommation d'API");
        }
    }
}
