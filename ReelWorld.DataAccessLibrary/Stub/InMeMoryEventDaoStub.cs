using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMeMoryEventDaoStub : IEventDao
    {
        private static List<Event> _events = new List<Event>();

        int IEventDao.Create(Event @event)
        {
            var newId = 1;
            if (_events.Any())
            {
                newId = _events.Max(@event => @event.EventId) + 1;

            }
            @event.EventId = newId;
            _events.Add(@event);
            return newId;
        }
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<int> IEventDao.CreateEventAsync(Event @event)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Event> IEventDao.GetAll()
        {
            throw new NotImplementedException();
        }

        Event? IEventDao.GetOne(int id)
        {
            throw new NotImplementedException();
        }

        bool IEventDao.Update(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
