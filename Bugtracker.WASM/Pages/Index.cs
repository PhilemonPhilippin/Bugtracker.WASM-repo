namespace Bugtracker.WASM.Pages
{
    public partial class Index
    {

        private bool _displayLogin;
        private bool _displayRegistration;

        private void DisplayLogin()
        {
            _displayRegistration = false;
            _displayLogin = true;
        }
        private void DisplayRegistration()
        {
            _displayLogin = false;
            _displayRegistration = true;
        }
    }
}
