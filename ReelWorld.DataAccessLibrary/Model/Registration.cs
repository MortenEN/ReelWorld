namespace ReelWorld.DataAccessLibrary.Model
{
    public class Registration
    {
        public int EventId { get; set; }
        public int ProfileId { get; set; }

        public Registration() { }

        public Registration(Event @event, Profile profile)
        {
            EventId = @event.EventId;
            ProfileId = profile.ProfileId;
        }
    }
}
