using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    internal class Event
    {
        #region Properties
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        #endregion

        #region Constructor
        public Event(int eventId, string title, string description, DateTime date, string location, string type)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Date = date;
            Location = location;
            Type = type;
        }
        public Event()
        {

        }
        #endregion
    }
}
