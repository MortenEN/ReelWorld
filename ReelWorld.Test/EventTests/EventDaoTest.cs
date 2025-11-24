using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;
using ReelWorld.DataAccessLibrary.Stub;
using System.Transactions;

namespace ReelWorld.Test;

public class EventDaoTest
{
    private IEventDaoAsync _dao;
    private const string connectionsString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";


    [SetUp]
    public void Setup()
    {
        _dao = new InMeMoryEventDaoStub();
    }

    [Test]
    public void EventDao_Create_Event_With_Stub()
    {
        //arrange
        var Event = new Event { EventId = 0, Title = "test" };
        //act
        var createdId = _dao.CreateAsync(Event);
        int createdIdInt = createdId.Result;
        //assert
        Assert.That(createdIdInt, Is.EqualTo(1));
        Assert.That(Event.EventId, Is.EqualTo(1));
    }

    [Test]
    public async Task EventDao_Create_Event_With_Database()
    {
        //arrange
        EventDao eventDao = new(connectionsString);
        var TestEvent = new Event
        {
            Title = "Test Event",
            Description = "This is a test event.",
            Location = "Test Location",
            Date = DateTime.Now,
            IsPublic = true,
            FK_Profile_Id = 1,
            Limit = 100
        };
        //act
        var createdId = await eventDao.CreateAsync(TestEvent);
        //assert
        Assert.That(createdId, Is.GreaterThan(0), "The Create method should return a EventId that is above 0");

    }

    [Test]
    public async Task EventDao_GetAll_Events_With_Database()
    {
        //arrange
        EventDao eventDao = new(connectionsString);
        //act
        var events = await eventDao.GetAllAsync();
        //assert
        Assert.That(events, Is.Not.Null, "The GetAll method should return a list of events");
        Assert.That(events.Count(), Is.GreaterThan(0), "The GetAll method should return at least one event");
    }

    //[Test]
    //public async Task EventDao_CreateUpdateDelete_Should_Work_Correctly()
    //{
    //    // Arrange
    //    EventDao eventDao = new EventDao(connectionsString);

    //    // 1. Create a new event
    //    var newEvent = new Event
    //    {
    //        Title = "Test Event",
    //        Description = "Test Description",
    //        Date = DateTime.Now.AddDays(1),
    //        Location = "Test Location",
    //        IsPublic = true,
    //        FK_Profile_Id = 1, // Replace with a valid profile ID in your DB
    //        Limit = 10
    //    };

    //    // Insert the event
    //    using var connection = new SqlConnection(connectionsString);
    //    await connection.OpenAsync();
    //    using var transaction = connection.BeginTransaction();
    //    try
    //    {
    //        var insertQuery = @"
    //        INSERT INTO [Event] (Title, Description, Date, Location, Visibility, FK_Profile_ID, [Limit])
    //        VALUES (@Title, @Description, @Date, @Location, @Visibility, @ProfileID, @Limit);
    //        SELECT CAST(SCOPE_IDENTITY() as int);";

    //        newEvent.EventId = await connection.ExecuteScalarAsync<int>(insertQuery, new
    //        {
    //            Title = newEvent.Title,
    //            Description = newEvent.Description,
    //            Date = newEvent.Date,
    //            Location = newEvent.Location,
    //            Visibility = newEvent.IsPublic,
    //            ProfileID = newEvent.FK_Profile_Id,
    //            Limit = newEvent.Limit
    //        }, transaction);

    //        // 2. Update the event
    //        newEvent.Title += " - Updated";
    //        newEvent.Description += " - Updated";
    //        newEvent.Location += " - Updated";
    //        newEvent.Date = newEvent.Date.AddDays(1);
    //        newEvent.Limit += 5;
    //        newEvent.IsPublic = !newEvent.IsPublic;

    //        var updateResult = await eventDao.UpdateAsync(newEvent);
    //        Assert.That(updateResult, Is.True, "UpdateAsync should return true");

    //        // Verify the update
    //        var updatedEvent = await eventDao.GetOneAsync(newEvent.EventId);
    //        Assert.That(updatedEvent.Title, Is.EqualTo(newEvent.Title));
    //        Assert.That(updatedEvent.Description, Is.EqualTo(newEvent.Description));
    //        Assert.That(updatedEvent.Location, Is.EqualTo(newEvent.Location));
    //        Assert.That(updatedEvent.Date, Is.EqualTo(newEvent.Date));
    //        Assert.That(updatedEvent.Limit, Is.EqualTo(newEvent.Limit));
    //        Assert.That(updatedEvent.IsPublic, Is.EqualTo(newEvent.IsPublic));

    //        // 3. Delete the event to clean up
    //        var deleteQuery = "DELETE FROM [Event] WHERE EventID = @EventID";
    //        var rowsDeleted = await connection.ExecuteAsync(deleteQuery, new { EventID = newEvent.EventId }, transaction);
    //        Assert.That(rowsDeleted, Is.EqualTo(1), "Event should be deleted");

    //        // Commit the transaction
    //        await transaction.CommitAsync();
    //    }
    //    catch
    //    {
    //        await transaction.RollbackAsync();
    //        throw;
    //    }
    //}
}
