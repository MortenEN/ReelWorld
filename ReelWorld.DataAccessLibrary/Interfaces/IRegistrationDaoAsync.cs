using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IRegistrationDaoAsync
    {
        Task<bool> JoinEventAsync(int eventId, int ProfileId);
        Task<Registration?> GetOneAsync(int id);

        Task<IEnumerable<Registration>> GetAllAsync();

        Task<int> CreateAsync(Registration registration);

        Task<bool> UpdateAsync(Registration registration);

        Task<bool> DeleteAsync(int id);
    }
}
