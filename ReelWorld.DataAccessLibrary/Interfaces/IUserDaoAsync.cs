using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IUserDaoAsync
    {
        Task<User?> GetOneAsync(int userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task<int> CreateAsync(User user);

        Task<bool> UpdateAsync(User user);

        Task<bool> DeleteAsync(int userId);
    }
}
