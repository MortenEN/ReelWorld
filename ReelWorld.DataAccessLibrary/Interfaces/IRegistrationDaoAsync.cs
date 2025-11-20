namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IRegistrationDaoAsync
    {
        Task<bool> JoinEventAsync(int eventId, int ProfileId);
    }
}
