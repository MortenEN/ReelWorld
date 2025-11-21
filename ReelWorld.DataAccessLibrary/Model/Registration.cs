namespace ReelWorld.DataAccessLibrary.Model
{
    public class Registration
    {
        public int EventId { get; set; }
        public int ProfileId { get; set; }

        public Registration() { }

        public Registration(int eventId, int profileId)
        {
            EventId = eventId;
            ProfileId = profileId;
        }
    }
}
