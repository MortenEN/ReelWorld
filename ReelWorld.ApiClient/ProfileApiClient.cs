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

        public Task<Profile?> GetOneAsync(int profileId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
