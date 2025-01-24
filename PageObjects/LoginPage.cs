using Microsoft.Playwright;

namespace PlaywrightTests.PageObjects;

public class LoginPage
{
    private readonly IPage _page;
    private readonly string _url = "https://the-internet.herokuapp.com/login";

    public ILocator FlashMessage => _page.Locator("#flash");

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync(_url);
    }

    public async Task LoginAsync(string username, string password)
    {
        await _page.FillAsync("input#username", username);
        await _page.FillAsync("input#password", password);
        await _page.ClickAsync("button[type='submit']");
    }
}
