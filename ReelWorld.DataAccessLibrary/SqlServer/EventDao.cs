using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class EventDao : BaseDao, IEventDao
    {
        public EventDao(string connectionString) : base(connectionString)
        {

        }

        public int Create(Event @event)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateEventAsync(Event @event)
        {
            throw new NotImplementedException();
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
