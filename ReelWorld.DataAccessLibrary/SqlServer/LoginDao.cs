using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Tools;
using System.Security.Cryptography;
using System.Text;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class LoginDao : BaseDao, ILoginDao
    {
        protected readonly string _connectionString;

        public LoginDao(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> LoginAsync(string email, string password)
        {
            try
            {
                var query = "SELECT ProfileID, HashPassword FROM Profile WHERE Email=@Email";
                using var connection = (SqlConnection)CreateConnection();
                await connection.OpenAsync();

                var profileTuple = await connection.QueryFirstOrDefaultAsync<ProfileTuple>(query, new { Email = email });
                if (profileTuple != null && BCryptTool.ValidatePassword(password, profileTuple.HashPassword))
                {
                    return profileTuple.ProfileID;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in for author with email {email}: '{ex.Message}'.", ex);
            }
        }

        internal class ProfileTuple
        {
            public int ProfileID { get; set; }
            public string HashPassword { get; set; }
        }
    }
}
