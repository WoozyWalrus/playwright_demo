using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using System.IO;
using System.Threading.Tasks;

namespace PlaywrightTests;

[TestClass]
public class FileUploadTests : PageTest
{
    [TestMethod]
    public async Task FileUploadWorksCorrectly()
    {
        // Navigate to the file upload page
        await Page.GotoAsync("https://the-internet.herokuapp.com/upload");

        // Define the file path relative to the project directory
        // (tests are run from C:\Users\USER\source\repos\PlaywrightTests\PlaywrightTests\bin\Debug\net8.0\)
        var filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, "Fixtures", "test_file.txt");

        // Upload the file
        await Page.SetInputFilesAsync("input#file-upload", filePath);

        // Click the upload button
        await Page.ClickAsync("input#file-submit");

        // Verify the file upload success message
        await Expect(Page.Locator("h3")).ToHaveTextAsync("File Uploaded!");
        await Expect(Page.Locator("#uploaded-files")).ToHaveTextAsync("test_file.txt");
    }
}