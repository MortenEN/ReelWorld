using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IEventDaoAsync
    {
        Task<Event?> GetOneAsync(int eventId);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> Get10LatestAsync();
        Task<IEnumerable<Event>> Get10BiggestAsync();
        Task<int> CreateAsync(Event @event);
        Task<bool> UpdateAsync(Event @event);
        Task<bool> DeleteAsync(int eventId);
        Task<IEnumerable<Event>> SearchAsync(string query);

    }
}
