using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMemoryUserDaoStub : IUserDao
    {
        private static List<User> _users = new List<User>();

        public int Create(User user)
        {
            var newId = 1;
            if (_users.Any())
            {
                newId = _users.Max(user => user.UserId) + 1;

            }
            user.UserId = newId;
            _users.Add(user);
            return newId;
        }

        public Task<int> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll() => _users;

        public User? GetOne(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
