using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class RegistrationDao : BaseDao, IRegistrationDaoAsync
    {
        public RegistrationDao(string connectionString) : base(connectionString)
        {
        }

        public Task<int> CreateAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> JoinEventAsync(int eventId, int profileId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

            try
            {
                // Check if User already joined
                var existsQuery = @"
                SELECT COUNT(*) 
                FROM EventProfile
                WHERE EventId = @EventId AND ProfileId = @ProfileId;";

                int count = await connection.ExecuteScalarAsync<int>(
                    existsQuery,
                    new { EventId = eventId, ProfileId = profileId },
                    transaction
                );

                if (count > 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // Insert new attendee
                var insertQuery = @"
                INSERT INTO EventProfile (EventId, ProfileId)
                VALUES (@EventId, @ProfileId);";

                int rows = await connection.ExecuteAsync(
                    insertQuery,
                    new { EventId = eventId, ProfileId = profileId },
                    transaction
                );

                transaction.Commit();
                return rows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<bool> UpdateAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Registration>> IRegistrationDaoAsync.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Registration?> IRegistrationDaoAsync.GetOneAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
