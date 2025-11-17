using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class Event
    {
        #region Properties
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public bool IsPublic { get; set; }
        #endregion

        #region Constructor
        public Event(int eventId, string title, string description, DateTime date, string location, bool isPublic)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Date = date;
            Location = location;
            IsPublic = isPublic;
        }
        public Event()
        {

        }
        #endregion
    }
}
