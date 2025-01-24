using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using PlaywrightTests.PageObjects;

namespace PlaywrightTests;

[TestClass]
public class loginTest: PageTest
{
    [TestMethod]
    public async Task SuccessfulTitle()
    {
        // Navigate to the login page
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");

        // Enter username
        await Page.FillAsync("input#username", "tomsmith");

        // Enter password
        await Page.FillAsync("input#password", "SuperSecretPassword!");

        // Click the login button
        await Page.ClickAsync("button[type='submit']");

        // Verify successful login message
        var flashMessage = Page.Locator("#flash");
        await Expect(flashMessage).ToContainTextAsync(new Regex("You logged into a secure area!"));

        // Verify that the URL has changed to the secure area
        await Expect(Page).ToHaveURLAsync("https://the-internet.herokuapp.com/secure");
    }
    [TestMethod]
    public async Task FailedLogin()
    {
        // Navigate to the login page
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");

        // Enter username
        await Page.FillAsync("input#username", "tomsmith");

        // Enter password
        await Page.FillAsync("input#password", "WrongPassword!");

        // Click the login button
        await Page.ClickAsync("button[type='submit']");

        // Verify successful login message
        var flashMessage = Page.Locator("#flash");
        await Expect(flashMessage).ToContainTextAsync(new Regex("Your password is invalid"));

        // Verify that the URL has changed to the secure area
        await Expect(Page).ToHaveURLAsync("https://the-internet.herokuapp.com/login");
    }

    [TestMethod]
    public async Task SuccessfulLoginUsingPOM()
    {
        // Initialize the Page Object Model
        var loginPage = new LoginPage(Page);

        // Navigate to the login page
        await loginPage.NavigateAsync();

        // Perform login with valid credentials
        await loginPage.LoginAsync("tomsmith", "SuperSecretPassword!");

        // Verify successfule login message
        await Expect(loginPage.FlashMessage).ToContainTextAsync("You logged into a secure area!");

        // Verify that the URL has changed to the secure area
        await Expect(Page).ToHaveURLAsync("https://the-internet.herokuapp.com/secure");
    }

    [TestMethod]
    public async Task UnsuccessfulLoginUsingPOM()
    {
        // Initialize the Page Object Model
        var loginPage = new LoginPage(Page);

        // Navigate to the login page
        await loginPage.NavigateAsync();

        // Perform login with invalid credentials
        await loginPage.LoginAsync("invalidUser", "invalidPass");

        // Verify the error message
        await Expect(loginPage.FlashMessage).ToContainTextAsync("Your username is invalid!");
    }
}
