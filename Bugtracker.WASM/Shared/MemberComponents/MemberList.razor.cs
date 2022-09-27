using Bugtracker.WASM.Models.MemberModels;
using Bugtracker.WASM.Tools;
using Microsoft.AspNetCore.Components;

namespace Bugtracker.WASM.Shared.MemberComponents
{
    public partial class MemberList : ComponentBase
    {
        [Inject]
        private IMemberLocalStorage LocalStorage { get; set; } = default!;
        [Inject]
        private IApiRequester Requester { get; set; } = default!;
        private List<MemberModel> _members = new List<MemberModel>();
        private MemberModel _memberTarget = new MemberModel() { IdMember = 0 };
        private MemberModel _myMemberModel = new();
        private int _myMemberId;
        private string _token;
        private bool _displayEditDialog;
        private bool _displayEditRoleDialog;
        private bool _displayDetailsDialog;
        private bool _isMemberConnected;

        protected override async Task OnInitializedAsync()
        {
            await RefreshMemberList();
            _myMemberId = await Requester.Get<int>("Member/idfromjwt", _token);
            _myMemberModel = await Requester.Get<MemberModel>($"Member/{_myMemberId}", _token);
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
                _displayEditRoleDialog = false;
                _memberTarget = member;
            }
        }

        private void DisplayEditDialog(MemberModel member)
        {
            _displayEditDialog = !_displayEditDialog;
            if (_displayEditDialog)
            {
                _displayDetailsDialog = false;
                _displayEditRoleDialog = false;
                _memberTarget = member;
            }
        }
        private void DisplayEditRoleDialog(MemberModel member)
        {
            _displayEditRoleDialog = !_displayEditRoleDialog;
            if (_displayEditRoleDialog)
            {
                _displayDetailsDialog = false;
                _displayEditDialog = false;
                _memberTarget = member;
            }
        }
        private async Task ConfirmEdit()
        {
            _displayEditDialog = false;
            _displayEditRoleDialog = false;
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
        private void CloseEditRoleDialog()
        {
            _displayEditRoleDialog = false;
        }
    }
}
