using Microsoft.Extensions.Logging;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test;

public class EventDaoTest
{
    private IEventDao _dao;

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
}
