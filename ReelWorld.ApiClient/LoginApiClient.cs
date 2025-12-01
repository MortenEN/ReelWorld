using ReelWorld.DataAccessLibrary.Model;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace ReelWorld.ApiClient
{
    public class LoginApiClient
    {
        private readonly RestClient _restClient;

        public LoginApiClient(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public async Task<int> LoginAsync(string email, string password)
        {
            var loginDto = new LoginDto
            {
                Email = email,
                Password = password
            };

            var request = new RestRequest("api/logins", Method.Post);
            request.AddJsonBody(loginDto);

            var response = await _restClient.ExecuteAsync<int>(request);

            if (response == null || !response.IsSuccessful)
            {
                throw new Exception($"Error logging in for email={email}. Response: {response?.Content}");
            }

            return response.Data;
        }
    }
}
