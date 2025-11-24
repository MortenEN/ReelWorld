using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class EventDao : BaseDao, IEventDaoAsync
    {
        public EventDao(string connectionString) : base(connectionString)
        {

        }

        public async Task<int> CreateAsync(Event @event)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Indsæt event
                var eventQuery = @"
                INSERT INTO [Event] (Title, Description, Date, Location, Visibility, FK_Profile_ID, Limit)
                OUTPUT INSERTED.EventID
                VALUES (@Title, @Description, @Date, @Location, @Visibility, @ProfileID, @Limit);
                ";

                var eventId = await connection.QuerySingleAsync<int>(eventQuery, new
                {
                    Title = @event.Title,
                    Description = @event.Description,
                    Date = @event.Date,
                    Location = @event.Location,
                    Visibility = @event.IsPublic,
                    ProfileID = @event.FK_Profile_Id,
                    Limit = @event.Limit
                }, transaction);

                transaction.Commit();
                return eventId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<bool> DeleteAsync(int eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "SELECT * FROM [Event]";
                return connection.Query<Event>(query).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Event>> Get10LatestAsync()
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "SELECT TOP 10 * FROM [Event] Order by eventId DESC";
                return connection.Query<Event>(query).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Event?> GetOneAsync(int eventId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "SELECT * FROM [Event] WHERE EventId = @EventId";
                var result = await connection.QuerySingleOrDefaultAsync<Event>(query, new { EventId = eventId });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Database error in GetOneAsync({eventId})", ex);
            }
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
        public async Task<bool> UpdateAsync(Event @event)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = @"
                UPDATE [Event]
                SET Title = @Title,
                    Description = @Description,
                    Date = @Date,
                    Location = @Location,
                    Visibility = @Visibility,
                    FK_Profile_ID = @ProfileID,
                    Limit = @Limit
                WHERE EventID = @EventID;";
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    Title = @event.Title,
                    Description = @event.Description,
                    Date = @event.Date,
                    Location = @event.Location,
                    Visibility = @event.IsPublic,
                    ProfileID = @event.FK_Profile_Id,
                    Limit = @event.Limit,
                    EventID = @event.EventId
                });
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
