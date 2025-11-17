using Microsoft.Extensions.Logging;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test;

public class EventDaoTest
{
    private IEventDao _dao;
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
        var createdId = _dao.Create(Event);
        //assert
        Assert.That(createdId, Is.EqualTo(1));
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
            FK_User_Id = 1
        };
        //act
        var createdId = await eventDao.CreateEventAsync(TestEvent);
        //assert
        Assert.That(createdId, Is.GreaterThan(0), "The Create method should return a EventId that is above 0");

    }
}
