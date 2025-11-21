using ReelWorld.DataAccessLibrary.Interfaces;
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
        #endregion
        public async Task<bool> JoinEventAsync(int eventId, int profileId)
        {
            var request = new RestRequest($"api/registrations/{eventId}", Method.Post);
            var response = await _restClient.ExecuteAsync<bool>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request - " + response.StatusCode);
            return response.Data;
        }
    }
}