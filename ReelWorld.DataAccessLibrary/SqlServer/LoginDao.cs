using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using ReelWorld.DataAccessLibrary.Interfaces;
using Dapper;

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
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            string sql = @"SELECT ProfileId FROM Profiles WHERE Email = @Email AND HashPassword = @HashPassword";
            using SqlCommand cmd = new(sql, connection);
            cmd.Parameters.AddWithValue("@Email", email);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read())
                return -1; // email findes ikke

            int profileId = reader.GetInt32(0);
            string storedHash = reader.GetString(1);

            if (!VerifyPassword(password, storedHash))
                return -1; // password forkert

            return profileId;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string hashedInput = HashPassword(password);
            return hashedInput == storedHash;
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
