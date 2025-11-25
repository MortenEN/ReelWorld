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
        //arrange
        var registrationDao = new RegistrationDao(connectionsString);
        var eventDao = new EventDao(connectionsString);

        var attendees = new List<Profile>();
        Event @event = new Event(0, "Test for joinAsync", "For testing joinAsync", DateTime.Now, "in .test", true, 1, 1, attendees);
        int eventId = await eventDao.CreateAsync(@event);

        int testUserId = 1;  // make sure user exists in test DB

        //act
        var result = await registrationDao.JoinEventAsync(eventId, testUserId);
        //assert
        Assert.That(result, Is.True, "JoinEventAsync should return true when user successfully joins an event.");
    }
    [Test]
    public async Task RegistrationDao_JoinEvent_Concurrency_With_Database()
    {
        //arrange
        var registrationDao = new RegistrationDao(connectionsString);
        var eventDao = new EventDao(connectionsString);

        var attendees = new List<Profile>();
        Event @event = new Event(0, "Test for concurrency", "For testing concurrency", DateTime.Now, "in .test", true, 1, 1, attendees);
        int eventId = await eventDao.CreateAsync(@event);

        int testUserId0 = 1;
        int testUserId1 = 2;

        //act
        var result0 = registrationDao.JoinEventAsync(eventId, testUserId0);
        var result1 = registrationDao.JoinEventAsync(eventId, testUserId1);

        await Task.WhenAll(result0, result1);
        var evt = await eventDao.GetOneAsync(eventId);
        //assert
        Assert.That(evt.AttendeeCount == 1, Is.True, "Only one profile joined the event.");
    }
}