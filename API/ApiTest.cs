using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json;

namespace PlaywrightTestPaylocity.API
{
    [TestFixture]
    internal class ApiTest
    {
        private HttpClient _httpClient;
        private const string RequestUrl = "https://wmxrwq14uc.execute-api.us-east-1.amazonaws.com/Prod/api/employees";
        private const string Username = "TestUser756";
        private const string Password = "FL;/EqAOa'.C";



        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{Username}:{Password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        [TearDown]
        public void Teardown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetEmployees_ShouldReturnSuccessStatusCode()
        {
            var response = await _httpClient.GetAsync(RequestUrl);
            Assert.That(response.IsSuccessStatusCode, Is.True, "API call to get employees did not return a success status code.");
        }

        [Test]
        public async Task AddEmployee_ShouldReturnSuccessStatusCode()
        {
            // Arrange
            var newEmployee = new
            {
                firstName = "Juan",
                lastName = "Perez",
                dependants = 1
            };

            var jsonPayload = JsonConvert.SerializeObject(newEmployee);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(RequestUrl, content);

            // Assert
            Assert.That(response.IsSuccessStatusCode, Is.True, "API call to add employee did not return a success status code.");
        }

        [Test]
        public async Task EditEmployee_ShouldUpdateFirstNameSuccessfully()
        {
            var newEmployee = new
            {
                firstName = "Juan",
                lastName = "Perez",
                dependants = 1
            };

            var jsonPayload = JsonConvert.SerializeObject(newEmployee);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var addResponse = await _httpClient.PostAsync(RequestUrl, content);
            Assert.That(addResponse.IsSuccessStatusCode, Is.True, "API call to add employee did not return a success status code.");

            var addedEmployeeContent = await addResponse.Content.ReadAsStringAsync();
            var addedEmployee = JsonConvert.DeserializeObject<Employee>(addedEmployeeContent);
            string employeeId = addedEmployee.Id;

            var updatedEmployee = new
            {
                id = employeeId,
                firstName = "JuanFromApiUpdated",
                lastName = "PerezFromApiUpdated",
                dependants = 3
            };

            var updatePayload = JsonConvert.SerializeObject(updatedEmployee);
            var updateContent = new StringContent(updatePayload, Encoding.UTF8, "application/json");
            var updateResponse = await _httpClient.PutAsync(RequestUrl, updateContent);

            Assert.That(updateResponse.IsSuccessStatusCode, Is.True, "API call to update employee did not return a success status code.");
        }

        [Test]
        public async Task AddAndDeleteEmployee_ShouldSucceed()
        {
            var newEmployee = new
            {
                firstName = "EmployeeToDelete",
                lastName = "EmployeeToDelete",
                dependants = 1
            };

            var jsonPayload = JsonConvert.SerializeObject(newEmployee);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var addResponse = await _httpClient.PostAsync(RequestUrl, content);
            Assert.That(addResponse.IsSuccessStatusCode, Is.True, "API call to add employee did not return a success status code.");

            var addedEmployeeContent = await addResponse.Content.ReadAsStringAsync();
            var addedEmployee = JsonConvert.DeserializeObject<Employee>(addedEmployeeContent);
            string employeeId = addedEmployee.Id;

            var deleteResponse = await _httpClient.DeleteAsync($"{RequestUrl}/{employeeId}");

            Assert.That(deleteResponse.IsSuccessStatusCode, Is.True, "API call to delete employee did not return a success status code.");
        }

        private class Employee
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Dependants { get; set; }
        }

    }
}
