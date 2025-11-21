using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IEventDaoAsync
    {
        Task<Event?> GetOneAsync(int eventId);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> Get10LatestAsync();
        Task<int> CreateAsync(Event @event);
        Task<bool> UpdateAsync(Event @event);
        Task<bool> DeleteAsync(int eventId);

    }
}
