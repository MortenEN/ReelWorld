using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMeMoryEventDaoStub : IEventDaoAsync
    {
        private static List<Event> _events = new List<Event>();

        public Task<bool> DeleteAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetOneAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Event @event)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(Event @event)
        {
            var newId = 1;
            if (_events.Any())
            {
                newId = _events.Max(@event => @event.EventId) + 1;

            }
            @event.EventId = newId;
            _events.Add(@event);
            return Task.FromResult(newId);
        }

        public Task<IEnumerable<Event>> Get10LatestAsync()
        {
            throw new NotImplementedException();
        }
    }
}
