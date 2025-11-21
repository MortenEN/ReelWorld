using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using RestSharp;

namespace ReelWorld.ApiClient
{
    public class RegistrationApiClient : IRegistrationDaoAsync
    {
        #region attributes and constructor
        //The address of the API server
        private readonly string _apiUri;

        //the rest client from restsharp to call the server
        private readonly RestClient _restClient;

        public RegistrationApiClient(string apiUri)
        {
            _restClient = new RestClient(apiUri);
            _apiUri = apiUri;
        }

        public Task<int> CreateAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Registration?> GetOneAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
        public async Task<bool> JoinEventAsync(int eventId, int profileId)
        {
            var request = new RestRequest($"api/registrations/{eventId}", Method.Post);
            var response = await _restClient.ExecuteAsync<bool>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request - " + response.StatusCode);
            return response.Data;
        }

        public Task<bool> UpdateAsync(Registration registration)
        {
            throw new NotImplementedException();
        }
    }
}