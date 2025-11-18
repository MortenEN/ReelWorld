using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class Event
    {
        #region Properties
        public int EventId { get; set; }
        [Display(Name = "Titel")]
        public string Title { get; set; }
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }
        [Display(Name = "Dato")]
        public DateTime Date { get; set; }
        [Display(Name = "Sted")]
        public string Location { get; set; }
        public bool IsPublic { get; set; }
        public int FK_User_Id { get; set; }
        [Display(Name = "Max antal deltagere")]
        public int Limit { get; set; }
        #endregion

        #region Constructor
        public Event(int eventId, string title, string description, DateTime date, string location, bool isPublic, int fk_User_Id, int limit)
        {
            EventId = eventId;
            Title = title;
            Description = description;
            Date = date;
            Location = location;
            IsPublic = isPublic;
            FK_User_Id = fk_User_Id;
            Limit = limit;
        }
        public Event()
        {

        }
        #endregion
    }
}
