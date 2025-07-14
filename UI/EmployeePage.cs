using Microsoft.Playwright;


namespace PlaywrightTestPaylocity.UI
{
    internal class EmployeePage
    {
        private readonly IPage _page;

        public EmployeePage(IPage page)
        {
            _page = page;
        }

        public async Task AddEmployeeAsync(string firstName, string lastName, string dependants)
        {
            await _page.ClickAsync("//button[text()='Add Employee']");
            await _page.FillAsync("//input[@id='firstName']", firstName);
            await _page.FillAsync("//input[@id='lastName']", lastName);
            await _page.FillAsync("//input[@id='dependants']", dependants);
            await _page.ClickAsync("//button[text()='Add']");
        }

        public async Task EditEmployeeAsync(string firstName, string lastName, string dependants, string newFirstName)
        {
            string rowXPath = $"//table[@id='employeesTable']//tr[td[text()='{lastName}'] and td[text()='{firstName}'] and td[text()='{dependants}']]";
            string editIconXPath = $"{rowXPath}//i[contains(@class, 'fa-edit')]";
            await _page.ClickAsync(editIconXPath);
            await _page.FillAsync("//input[@id='firstName']", newFirstName);
            await _page.ClickAsync("//button[text()='Update']");
        }

        public async Task DeleteEmployeeAsync(string firstName, string lastName, string dependants)
        {
            string rowXPath = $"//table[@id='employeesTable']//tr[td[text()='{lastName}'] and td[text()='{firstName}'] and td[text()='{dependants}']]";
            string deleteIconXPath = $"{rowXPath}//i[contains(@class, 'fa-times')]";
            await _page.ClickAsync(deleteIconXPath);
            await _page.ClickAsync("//button[text()='Delete']");
        }

    }
}
