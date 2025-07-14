using Microsoft.Playwright;


namespace PlaywrightTestPaylocity.UI
{
    internal class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public async Task NavigateAsync()
        {
            await _page.GotoAsync("https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/Account/Login");
        }

        public async Task LoginAsync(string username, string password)
        {
            await _page.FillAsync("//input[@name='Username']", username);
            await _page.FillAsync("//input[@name='Password']", password);
            await _page.ClickAsync("text='Log In'");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}
