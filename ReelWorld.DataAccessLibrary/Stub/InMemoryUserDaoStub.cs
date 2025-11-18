using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMemoryUserDaoStub : IUserDaoAsync
    {
        private static List<User> _users = new List<User>();

        public Task<int> CreateAsync(User user)
        {
            var newId = 1;
            if (_users.Any())
            {
                newId = _users.Max(user => user.UserId) + 1;

            }
            user.UserId = newId;
            _users.Add(user);
            return Task.FromResult(newId);
        }

        public Task<bool> DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetOneAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
