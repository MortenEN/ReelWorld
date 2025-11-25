using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface IProfileDaoAsync
    {
        Task<Profile?> GetOneAsync(int id);

        Task<IEnumerable<Profile>> GetAllAsync();

        Task<int> CreateAsync(Profile profile);

        Task<bool> UpdateAsync(Profile profile);

        Task<bool> DeleteAsync(int id);
        Task<int> LoginAsync(string email, string password);
    }
}
