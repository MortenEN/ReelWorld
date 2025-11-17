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
    public class EventDao : BaseDao, IEventDao
    {
        public EventDao(string connectionString) : base(connectionString)
        {

        }

        public int Create(Event @event)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateEventAsync(Event @event)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Indsæt event
                var eventQuery = @"
                INSERT INTO [Event] (Title, Description, Date, Location, Visibility, FK_User_ID)
                OUTPUT INSERTED.EventID
                VALUES (@Title, @Description, @Date, @Location, @Visibility, @UserID);
                ";

                var eventId = await connection.QuerySingleAsync<int>(eventQuery, new
                {
                    Title = @event.Title,
                    Description = @event.Description,
                    Date = @event.Date,
                    Location = @event.Location,
                    Visibility = @event.IsPublic,
                    UserID = @event.FK_User_Id
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

        public bool Delete(int eventid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetAll()
        {
            throw new NotImplementedException();
        }

        public Event? GetOne(int eventId)
        {
            throw new NotImplementedException();
        }

        public bool Update(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
