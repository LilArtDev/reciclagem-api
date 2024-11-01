using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using RestSharp;
using Xunit;

namespace ReciclagemApi.Tests.StepDefinitions
{
    [Binding]
    public class ApiStepDefinitions
    {
        private readonly ScenarioContext _context;
        private readonly RestClient _client;
        private RestResponse _response;

        public ApiStepDefinitions(ScenarioContext context)
        {
            _context = context;
            _client = new RestClient("http://localhost:5074/api/");
        }

        [BeforeScenario("RequireAuth")]
        public async Task BeforeScenarioWithAuth()
        {
            var token = await AuthenticateUser("authTest", "testing22", "User");
            _context["token"] = token;
        }

        [BeforeScenario("RequireAdminAuth")]
        public async Task BeforeScenarioWithAdminAuth()
        {
            var token = await AuthenticateUser("test_admin", "test_password", "Admin");
            _context["adminToken"] = token;
        }

        [Given(@"I have the following user details")]
        public void GivenIHaveTheFollowingUserDetails(Table table)
        {
            var userDetails = (IDictionary<string, object>)table.CreateDynamicInstance();
            _context["userDetails"] = new
            {
                username = userDetails["Username"].ToString(),
                password = userDetails["Password"].ToString(),
                role = userDetails["Role"].ToString()
            };
        }

        [When(@"I register a new user")]
        public async Task WhenIRegisterANewUser()
        {
            var request = new RestRequest("auth/register", Method.Post);
            request.AddJsonBody(_context["userDetails"]);
            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            Assert.True(_response != null, "Response is null.");
            Assert.Equal((HttpStatusCode)expectedStatusCode, _response.StatusCode);
        }

        [Given(@"I have valid user credentials")]
        public void GivenIHaveValidUserCredentials(Table table)
        {
            var credentials = (IDictionary<string, object>)table.CreateDynamicInstance();
            _context["credentials"] = new
            {
                username = credentials["Username"].ToString(),
                password = credentials["Password"].ToString()
            };
        }

        [Given(@"I have Admin user credentials")]
        public async Task GivenIHaveAdminUserCredentials(Table table)
        {
            var adminCredentials = table.CreateDynamicInstance();
            _context["adminCredentials"] = new
            {
                username = ((IDictionary<string, object>)adminCredentials)["Username"]?.ToString(),
                password = ((IDictionary<string, object>)adminCredentials)["Password"]?.ToString()
            };

            var credentials = (dynamic)_context["adminCredentials"];
            var token = await AuthenticateUser(credentials.username, credentials.password);
            _context["adminToken"] = token;

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Failed to retrieve Admin JWT token.");
            }
        }

        [When(@"I login with these credentials")]
        public async Task WhenILoginWithTheseCredentials()
        {
            var credentials = _context.ContainsKey("adminCredentials") ? _context["adminCredentials"] : _context["credentials"];
            var request = new RestRequest("auth/login", Method.Post);
            request.AddJsonBody(credentials);
            _response = await _client.ExecuteAsync(request);

            if (_response.IsSuccessful && _response.Content != null)
            {
                var jsonResponse = JsonDocument.Parse(_response.Content);
                if (jsonResponse.RootElement.TryGetProperty("token", out var token))
                {
                    _context["token"] = token.GetString();
                }
                else
                {
                    throw new InvalidOperationException("Token not found in response.");
                }
            }
            else
            {
                throw new InvalidOperationException("Failed to retrieve JWT token during login.");
            }
        }

        [Then(@"I should receive a JWT token")]
        public void ThenIShouldReceiveAJWTToken()
        {
            Assert.True(_context.ContainsKey("token") || _context.ContainsKey("adminToken"), "No JWT token available.");
        }

        [Given(@"I have a valid JWT token")]
        public void GivenIHaveAValidJWTToken()
        {
            Assert.True(_context.ContainsKey("token"), "No JWT token available. Ensure that the login step was successful.");
        }

        [Given(@"I have the following recycling report")]
        public void GivenIHaveTheFollowingRecyclingReport(Table table)
        {
            var report = (IDictionary<string, object>)table.CreateDynamicInstance();
            _context["recyclingReport"] = new
            {
                material = report["Material"].ToString(),
                quantity = report["Quantity"].ToString()
            };
        }

        [When(@"I submit a recycling report")]
        public async Task WhenISubmitARecyclingReport()
        {
            var request = new RestRequest("report", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_context["token"]}");
            request.AddJsonBody(_context["recyclingReport"]);
            _response = await _client.ExecuteAsync(request);
        }

        [Given(@"I have the following material details")]
        public void GivenIHaveTheFollowingMaterialDetails(Table table)
        {
            var materialDetails = (IDictionary<string, object>)table.CreateDynamicInstance();
            _context["materialDetails"] = new
            {
                name = materialDetails["MaterialName"].ToString(),
                description = materialDetails["Description"].ToString()
            };
        }

        [When(@"I submit a material creation request")]
        public async Task WhenISubmitAMaterialCreationRequest()
        {
            var request = new RestRequest("material", Method.Post);
            var token = _context.ContainsKey("adminToken") ? _context["adminToken"] : _context["token"];
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(_context["materialDetails"]);
            _response = await _client.ExecuteAsync(request);
        }

        private async Task<string> AuthenticateUser(string username, string password, string role = "User")
        {
            var credentials = new { username, password };
            var request = new RestRequest("auth/login", Method.Post);
            request.AddJsonBody(credentials);

            _response = await _client.ExecuteAsync(request);

            if (!_response.IsSuccessful || _response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RegisterUser(username, password, role);

                _response = await _client.ExecuteAsync(request);
                if (!_response.IsSuccessful)
                {
                    throw new InvalidOperationException("Failed to retrieve JWT token after creating user.");
                }
            }

            if (_response.Content != null)
            {
                var jsonResponse = JsonDocument.Parse(_response.Content);
                if (jsonResponse.RootElement.TryGetProperty("token", out var token))
                {
                    return token.GetString(); // Retorna o token JWT
                }
            }

            throw new InvalidOperationException("Token not found in response.");
        }

        private async Task RegisterUser(string username, string password, string role)
        {
            var userDetails = new { username, password, role };
            var request = new RestRequest("auth/register", Method.Post);
            request.AddJsonBody(userDetails);

            var response = await _client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return;
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return;
            }
            else
            {
                throw new InvalidOperationException("Failed to create user: " + response.Content);
            }
        }
    }
}
