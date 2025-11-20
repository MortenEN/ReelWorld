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
                INSERT INTO [Event] (Title, Description, Date, Location, Visibility, FK_User_ID, Limit)
                OUTPUT INSERTED.EventID
                VALUES (@Title, @Description, @Date, @Location, @Visibility, @UserID, @Limit);
                ";

                var eventId = await connection.QuerySingleAsync<int>(eventQuery, new
                {
                    Title = @event.Title,
                    Description = @event.Description,
                    Date = @event.Date,
                    Location = @event.Location,
                    Visibility = @event.IsPublic,
                    UserID = @event.FK_User_Id,
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

        public async Task<bool> JoinEventAsync(int eventId, int userId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {

                var existsQuery = @"
                SELECT COUNT(*) 
                FROM EventUser
                WHERE EventId = @EventId AND UserId = @UserId;";

                int count = await connection.ExecuteScalarAsync<int>(
                    existsQuery,
                    new { EventId = eventId, UserId = userId },
                    transaction
                );

                if (count > 0)
                {
                    transaction.Rollback();
                    return false; // User already joined
                }

                // Insert new attendee
                var insertQuery = @"
                INSERT INTO EventUser (EventId, UserId)
                VALUES (@EventId, @UserId);";

                int rows = await connection.ExecuteAsync(
                    insertQuery,
                    new { EventId = eventId, UserId = userId },
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
        public Task<bool> UpdateAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
