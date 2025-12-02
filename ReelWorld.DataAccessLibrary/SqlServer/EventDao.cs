using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

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

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "DELETE FROM [Event] WHERE EventID = @EventID";
                var affectedRows = await connection.ExecuteAsync(query, new { EventID = id });
                return affectedRows > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = @"
            SELECT e.EventID, e.Title, e.Description, e.Date, e.Location, e.Visibility, e.FK_Profile_ID,e.Limit,
            (SELECT COUNT(*) FROM EventProfile ep WHERE ep.EventId = e.EventID) AS AttendeeCount
            FROM [Event] e;";
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
                var query = @" 
            SELECT TOP 10 e.EventID, e.Title, e.Description, e.Date, e.Location, e.Visibility, e.FK_Profile_ID, e.Limit,
            (SELECT COUNT(*) FROM EventProfile ep WHERE ep.EventId = e.EventID) AS AttendeeCount
            FROM [Event] e
            ORDER BY e.EventID DESC;";
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
                var query = @"
            SELECT e.EventID, e.Title, e.Description, e.Date, e.Location, e.Visibility, e.FK_Profile_ID, e.Limit,
            (SELECT COUNT(*) FROM EventProfile ep WHERE ep.EventId = e.EventID) AS AttendeeCount
            FROM [Event] e
            WHERE e.EventID = @EventId;";
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

            using var transaction = await connection.BeginTransactionAsync();

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
                }, transaction); // Pass the transaction

                await transaction.CommitAsync(); // Commit if successful

                return rowsAffected > 0;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Rollback on error
                throw;
            }
        }

        public async Task<List<Event>> SearchAsync(string query)
        {
            using var connection = (SqlConnection)CreateConnection();
            var sql = @"SELECT * FROM Event
                WHERE Title LIKE @Query
                   OR Description LIKE @Query";

            return (await connection.QueryAsync<Event>(sql, new { Query = "%" + query + "%" })).ToList();
        }

        Task<IEnumerable<Event>> IEventDaoAsync.SearchAsync(string query)
        {
            throw new NotImplementedException();
        }
    }
}

