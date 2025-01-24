using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaywrightTests;

[TestClass]
public class ApiMockingTests : PageTest
{
    [TestMethod]
    public async Task ApiMockingExample()
    {
        // Intercept the API request and provide a mock response
        await Page.RouteAsync("https://reqres.in/api/users/2", async route =>
        {
            var mockResponse = new
            {
                data = new
                {
                    id = 2,
                    email = "janet.weaver@reqres.in",
                    first_name = "Janet",
                    last_name = "Weaver",
                    avatar = "https://reqres.in/img/faces/2-image.jpg"
                }
            };

            var json = JsonSerializer.Serialize(mockResponse);

            await route.FulfillAsync(new RouteFulfillOptions
            {
                ContentType = "application/json",
                Body = json
            });
        });

        // Make a fetch request on the page
        await Page.GotoAsync("about:blank"); // Blank page for demonstration
        var response = await Page.EvaluateAsync<JsonElement>(@"async () => {
            const res = await fetch('https://reqres.in/api/users/2');
            return await res.json();
        }");

        // Verify the mocked response
        Assert.AreEqual("Janet", response.GetProperty("data").GetProperty("first_name").GetString());
    }
}
