using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Test;

public class RegistrationDaoTest
{
    private IRegistrationDaoAsync _dao;
    private IEventDaoAsync _eventDaoAsync;
    private const string connectionsString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task RegistrationDao_JoinEvent_With_Database()
    {
        var registrationDao = new RegistrationDao(connectionsString);
        var eventDao = new EventDao(connectionsString);

        var attendees = new List<Profile>();
        var ev = new Event(0,
            "Test for joinAsync",
            "For testing joinAsync",
            DateTime.Now,
            "in .test",
            true,
            1,
            1,
            attendees);

        using var connection = new SqlConnection(connectionsString);
        await connection.OpenAsync();

        int eventId = await eventDao.CreateAsync(ev);
        int testUserId = 1; // must exist

        try
        {
            // act
            var result = await registrationDao.JoinEventAsync(eventId, testUserId);

            // assert
            Assert.That(result, Is.True);
        }
        finally
        {
            // CLEANUP: remove join entry + event
            await connection.ExecuteAsync(
                "DELETE FROM EventProfile WHERE EventId = @EventId AND ProfileId = @ProfileId",
                new { EventId = eventId, ProfileId = testUserId });

            await connection.ExecuteAsync(
                "DELETE FROM Event WHERE EventId = @EventId",
                new { EventId = eventId });
        }
    }

    [Test]
    public async Task RegistrationDao_JoinEvent_Concurrency_With_Database()
    {
        var registrationDao = new RegistrationDao(connectionsString);
        var eventDao = new EventDao(connectionsString);

        var attendees = new List<Profile>();
        var ev = new Event(0,
            "Test for concurrency",
            "For testing concurrency",
            DateTime.Now,
            "in .test",
            true,
            1,
            1,
            attendees);

        using var connection = new SqlConnection(connectionsString);
        await connection.OpenAsync();

        int eventId = await eventDao.CreateAsync(ev);

        int testUserId0 = 1;
        int testUserId1 = 2;

        try
        {
            var t0 = registrationDao.JoinEventAsync(eventId, testUserId0);
            var t1 = registrationDao.JoinEventAsync(eventId, testUserId1);

            await Task.WhenAll(t0, t1);

            var evt = await eventDao.GetOneAsync(eventId);

            Assert.That(evt.AttendeeCount == 1,
                "Only one profile should be able to join when event limit = 1.");
        }
        finally
        {
            // cleanup: delete all entries related to this event
            await connection.ExecuteAsync(
                "DELETE FROM EventProfile WHERE EventId = @EventId",
                new { EventId = eventId });

            await connection.ExecuteAsync(
                "DELETE FROM Event WHERE EventId = @EventId",
                new { EventId = eventId });
        }
    }
}