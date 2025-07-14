using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTestPaylocity.UI
{
    [TestFixture]
    public class EmployeeTests
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private LoginPage? _loginPage;
        private EmployeePage? _employeePage;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _page = await _browser.NewPageAsync();

            _loginPage = new LoginPage(_page);
            _employeePage = new EmployeePage(_page);

            await _loginPage.NavigateAsync();
            await _loginPage.LoginAsync("TestUser756", "FL;/EqAOa'.C");

            Assert.That(await _page.InnerTextAsync("//a[@class='navbar-brand' and @href='/Prod/Benefits' and text()='Paylocity Benefits Dashboard']"), Is.EqualTo("Paylocity Benefits Dashboard"));
        }

        [TearDown]
        public async Task Teardown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [Test]
        public async Task AddEmployeeTest()
        {
            await _employeePage.AddEmployeeAsync("AddedEmployee", "Ruvalcaba", "1");
            Assert.That(await _page.InnerTextAsync("//td[text()='AddedEmployee']"), Does.Contain("AddedEmployee"));
        }

        [Test]
        public async Task EditEmployeeTest()
        {
            await _employeePage.AddEmployeeAsync("Employee", "Ruvalcaba", "1");
            await _employeePage.EditEmployeeAsync("Employee", "Ruvalcaba", "1", "EmployeeUpdated");
            Assert.That(await _page.InnerTextAsync("//td[text()='EmployeeUpdated']"), Does.Contain("EmployeeUpdated"));
        }

        [Test]
        public async Task DeleteEmployeeTest()
        {
            await _employeePage.AddEmployeeAsync("EmployeeToDelete", "Ruvalcaba", "1");
            await _employeePage.DeleteEmployeeAsync("EmployeeToDelete", "Ruvalcaba", "1");
            await _page.WaitForSelectorAsync("//td[text()='EmployeeToDelete']", new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached });
            var elements = await _page.QuerySelectorAllAsync("//td[text()='EmployeeToDelete']");
            Assert.That(elements.Count, Is.EqualTo(0));
        }
    }
}
