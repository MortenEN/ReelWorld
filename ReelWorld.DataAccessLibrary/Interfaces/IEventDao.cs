using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IEventDao
    {
        Event? GetOne(int eventId);

        IEnumerable<Event> GetAll();

        int Create(Event @event);

        bool Update(Event @event);

        bool Delete(int eventid);

        bool JoinEventAsync(int EventId,int ProfileId);
    }
}
