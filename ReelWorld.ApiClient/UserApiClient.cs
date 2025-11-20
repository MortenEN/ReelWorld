using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using RestSharp;

namespace ReelWorld.ApiClient
{
    public class UserApiClient : IUserDaoAsync
    {
        #region attributes and constructor
        //The address of the API server
        private readonly string _apiUri;

        //the rest client from restsharp to call the server
        private readonly RestClient _restClient;

        public UserApiClient(string apiUri)
        {
            _apiUri = apiUri;
            _restClient = new RestClient(apiUri);
        }
        #endregion

        public async Task<int> CreateAsync(User user)
        {
            var request = new RestRequest("api/users", Method.Post);
            request.AddJsonBody(user);

            var response = await _restClient.ExecuteAsync<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var request = new RestRequest("api/users/{id}", Method.Delete);
            var response = await _restClient.ExecuteAsync(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return true;
        }


        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var request = new RestRequest("api/users", Method.Get);
            var response = await _restClient.ExecuteAsync<IEnumerable<User>>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public Task<User?> GetOneAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}