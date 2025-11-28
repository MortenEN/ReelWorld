using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using RestSharp;

namespace ReelWorld.ApiClient
{
    public class ProfileApiClient : IProfileDaoAsync
    {
        #region attributes and constructor
        // The address of the API server
        private readonly string _apiUri;

        // The RestClient from RestSharp to call the server
        private readonly RestClient _restClient;

        public ProfileApiClient(string apiUri)
        {
            _apiUri = apiUri;
            _restClient = new RestClient(apiUri);
        }
        #endregion

        public async Task<int> CreateAsync(Profile profile)
        {
            var request = new RestRequest("api/profiles", Method.Post);
            request.AddJsonBody(profile);

            var response = await _restClient.ExecuteAsync<int>(request);
            if (response == null) throw new Exception("No response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public async Task<bool> DeleteAsync(int profileId)
        {
            var request = new RestRequest("api/profiles/{id}", Method.Delete);
            request.AddUrlSegment("id", profileId);
            var response = await _restClient.ExecuteAsync(request);
            if (response == null) throw new Exception("No response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return true;
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            var request = new RestRequest("api/profiles", Method.Get);
            var response = await _restClient.ExecuteAsync<IEnumerable<Profile>>(request);
            if (response == null) throw new Exception("No response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public async Task<Profile?> GetOneAsync(int profileId)
        {
            var request = new RestRequest($"api/profiles/{profileId}", Method.Get);
            var response = await _restClient.ExecuteAsync<Profile>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request - " + response.StatusCode);
            return response.Data;
        }

        public async Task<int> LoginAsync(string email, string password)
        {
            var request = new RestRequest("api/logins", Method.Post);
            // Add email and password to the request body
            request.AddJsonBody(new { Email = email, Password = password });

            var response = await _restClient.ExecuteAsync<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request - " + response.StatusCode);
            return response.Data;
        }

        public async Task<bool> UpdateAsync(Profile profile)
        {
            var request = new RestRequest($"api/profiles/{profile.ProfileId}", Method.Put);
            request.AddJsonBody(profile);
            var response = await _restClient.ExecuteAsync(request);
            if (response == null) throw new Exception("No response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return true;
        }

        public async Task<int> LoginAsync(Profile profile)
        {
            var request = new RestRequest("logins", Method.Post);
            request.AddJsonBody(profile);
            var response = await _restClient.ExecuteAsync<int>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error logging in for author with email={profile.Email}. Message was {response.Content}");
            }
            return response.Data;
        }
    }
}
