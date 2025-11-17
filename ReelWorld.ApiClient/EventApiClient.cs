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
    public class EventApiClient : IEventDao
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

        public int Create(Event @event)
        {
            var request = new RestRequest("api/events", Method.Post);
            request.AddJsonBody(@event);

            var response = _restClient.Execute<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception("Server reply: Unsuccessful request");
            return response.Data;
        }

        public async Task<int> CreateEventAsync(Event @event)
        {
            var request = new RestRequest("api/events", Method.Post);
            request.AddJsonBody(@event);

            var response = await _restClient.ExecuteAsync<int>(request);
            if (response == null) throw new Exception("NO response from server");
            if (!response.IsSuccessStatusCode) throw new Exception($"Server reply: Unsuccessful request - {response.StatusCode}");
            return response.Data;
        }

        public bool Delete(int eventid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetAll()
        {
            throw new NotImplementedException();
        }

        public Event? GetOne(int eventId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
