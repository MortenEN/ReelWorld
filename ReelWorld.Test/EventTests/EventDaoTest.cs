using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.SqlServer;
using ReelWorld.DataAccessLibrary.Stub;
using Event = ReelWorld.DataAccessLibrary.Model.Event;

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
        // arrange
        var eventDao = new EventDao(connectionsString);

        var testEvent = new Event
        {
            Title = "Test Event",
            Description = "This is a test event.",
            Location = "Test Location",
            Date = DateTime.Now,
            IsPublic = true,
            FK_Profile_Id = 1,
            Limit = 100
        };

        int createdId = 0;

        try
        {
            // act
            createdId = await eventDao.CreateAsync(testEvent);

            // assert
            Assert.That(createdId, Is.GreaterThan(0),
                "CreateAsync should return a valid generated EventId.");
        }
        finally
        {
            if (createdId > 0)
            {
                using var connection = new SqlConnection(connectionsString);
                await connection.OpenAsync();

                // CLEANUP: remove created event
                await connection.ExecuteAsync(
                    "DELETE FROM Event WHERE EventId = @Id",
                    new { Id = createdId });
            }
        }
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

    [Test]
    public async Task EventDao_CreateUpdateDelete_ShouldWorkCorrectly()
    {
        //arrange
        var eventDao = new EventDao(connectionsString);

        var newEvent = new Event
        {
            Title = "Test Event",
            Description = "Test Description",
            Date = DateTime.Now.AddDays(1),
            Location = "Test Location",
            IsPublic = true,
            FK_Profile_Id = 1,
            Limit = 10
        };

        using var connection = new SqlConnection(connectionsString);
        await connection.OpenAsync();

        newEvent.EventId = await connection.ExecuteScalarAsync<int>(@"
        INSERT INTO [Event] (Title, Description, Date, Location, Visibility, fk_profile_id, [Limit])
        VALUES (@Title, @Description, @Date, @Location, @Visibility, @ProfileId, @Limit);
        SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new
            {
                Title = newEvent.Title,
                Description = newEvent.Description,
                Date = newEvent.Date,
                Location = newEvent.Location,
                Visibility = newEvent.IsPublic,
                ProfileId = newEvent.FK_Profile_Id,
                Limit = newEvent.Limit
            });

        //act
        newEvent.Title += " Updated";
        newEvent.Description += " Updated";
        newEvent.Location += " Updated";
        newEvent.IsPublic = !newEvent.IsPublic;
        newEvent.Limit += 10;

        //assert
        Assert.That(await eventDao.UpdateAsync(newEvent), Is.True);

        var updated = await eventDao.GetOneAsync(newEvent.EventId);

        Assert.That(updated.Title, Is.EqualTo(newEvent.Title));

        await connection.ExecuteAsync("DELETE FROM [Event] WHERE EventId = @Id",
            new { Id = newEvent.EventId });
    }
    [Test]
    public async Task EventDao_SearchAsync_Events_With_Database()
    {
        //arrange
        int eventId = 0;
        using var connection = new SqlConnection(connectionsString);
        EventDao eventDao = new(connectionsString);
        Event testEvent = new Event
        {
            Title = "Test Search Event",
            Description = "This is a for search event.",
            Location = ".test",
            Date = DateTime.Now,
            IsPublic = true,
            FK_Profile_Id = 1,
            Limit = 100
        };
        try
        {

            //act
            eventId = await eventDao.CreateAsync(testEvent);

            List<Event> res = await eventDao.SearchAsync("Test Search Event");
            //assert
            Assert.That(res.Count == 1);
        }
        finally
        {
            // CLEANUP: remove event
            await connection.ExecuteAsync(
                "DELETE FROM Event WHERE EventId = @EventId",
                new { EventId = eventId });
        }
    }
}
