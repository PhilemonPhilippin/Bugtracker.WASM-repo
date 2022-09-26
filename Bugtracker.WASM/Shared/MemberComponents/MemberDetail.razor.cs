using Bugtracker.WASM.Models.MemberModels;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberDetail
    {
        [Parameter]
        public MemberModel MemberTarget { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }

        private string RoleName(int role)
        {
            switch (role)
            {
                case 2:
                    return "Developer";
                case 3:
                    return "Project Manager";
                case 4:
                    return "Administrator";
                case 5:
                    return "Web Master";
                default:
                    return "Tester";
            }
        }
    }
}
