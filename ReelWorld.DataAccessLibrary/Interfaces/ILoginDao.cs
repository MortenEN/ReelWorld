using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    public interface ILoginDao
    {
        Task<int> LoginAsync(string email, string password);
    }
}
