using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.ApiClient
{
    public class EventApiClient : IEventDaoAsync
    {
        #region attributes and constructor
        //The address of the API server
        private readonly string _apiUri;

        //the rest client from restsharp to call the server
        private readonly RestClient _restClient;

        public EventApiClient(string apiUri)
        {
            _restClient = new RestClient(apiUri);
            _apiUri = apiUri;
        }
        #endregion

        public async Task<int> CreateAsync(Event @event)
        {
            var request = new RestRequest("api/events", Method.Post);
            request.AddJsonBody(@event);

            var response = await _restClient.ExecuteAsync<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public Task<bool> DeleteAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            var request = new RestRequest("api/events", Method.Get);
            var response = await _restClient.ExecuteAsync<IEnumerable<Event>>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public Task<Event?> GetOneAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
