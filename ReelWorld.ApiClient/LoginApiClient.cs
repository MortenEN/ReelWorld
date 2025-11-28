using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.ApiClient
{
    public class LoginApiClient
    {
        private readonly HttpClient _http;

        public LoginApiClient(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl)};
        }

        public async Task<int> LoginAsync(string email, string password)
        {
            var profile = new Profile
            {
                Email = email,
                HashPassword = password
            };

            var response = await _http.PostAsJsonAsync("api/logins/login", profile);

            if (!response.IsSuccessStatusCode)
                return -1;

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
