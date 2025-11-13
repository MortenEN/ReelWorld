using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IUserDao
    {
        User? GetOne(int id);
        IEnumerable<User> GetAll();
        int Create(User user);
        bool Update(User user);
        bool Delete(int id);
    }
}