using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Interfaces;
using RestSharp;

namespace ReelWorld.ApiClient
{
    public class UserApiClient : IUserDao
    {
        #region attributes and constructor
        //The address of the API server
        private readonly string _apiUri;

        //the rest client from restsharp to call the server
        private readonly RestClient _restClient;

        public UserApiClient(string apiUri)
        {
            _restClient = new RestClient(apiUri);
            _apiUri = apiUri;
        }
        #endregion

        public int Create(User user)
        {
            var request = new RestRequest("users", Method.Post);
            request.AddJsonBody(user);

            var response = _restClient.Execute<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request");
            return response.Data;
        }

        public bool Delete(int id)
        {
            
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            var request = new RestRequest("users", Method.Get);
            var response = _restClient.Execute<IEnumerable<User>>(request);
            if (response == null) throw new Exception("NO response from server");
            if (response.IsSuccessStatusCode) return response.Data;
            throw new Exception("Server reply: Unsuccessful request");
        }

        public User? GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}