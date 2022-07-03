namespace Bugtracker.WASM.Pages
{
    public partial class Dashboard
    {
        public async Task RefreshComponent()
        {
            await InvokeAsync(StateHasChanged);
        }
    }
}
