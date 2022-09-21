using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberList
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<MemberModel> _members = new List<MemberModel>();
        private MemberModel _memberTarget = new MemberModel() { IdMember = 0 };
        private string _token;
        private bool _displayEditDialog;
        private bool _displayDetailsDialog;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            await RefreshMemberList();
        }
        private async Task RefreshMemberList()
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                _token = await LocalStorage.GetToken();
                _members = await Requester.Get<List<MemberModel>>("Member", _token);
            }
        }
        private async Task DeleteMember(int id)
        {
            _token = await LocalStorage.GetToken();
            if (_token is not null)
            {
                _isMemberConnected = true;
                await Requester.Delete($"Member/{id}", _token);
                await RefreshMemberList();
            }
        }
        private void DisplayDetailsDialog(MemberModel member)
        {
            _displayDetailsDialog = !_displayDetailsDialog;
            if (_displayDetailsDialog)
            {
                _displayEditDialog = false;
                _memberTarget = member;
            }
        }

        private void DisplayEditDialog(MemberModel member)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayDetailsDialog = false;
                _memberTarget = member;
            }
        }
        private async Task ConfirmEdit()
        {
            _displayEditDialog = false;
            await RefreshMemberList();
        }
        private void CloseDetailsDialog()
        {
            _displayDetailsDialog = false;
        }
        private void CloseEditDialog()
        {
            _displayEditDialog = false;
        }
    }
}
