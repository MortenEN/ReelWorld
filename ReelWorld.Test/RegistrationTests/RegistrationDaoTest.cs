using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Test;

public class RegistrationDaoTest
{
    private IRegistrationDaoAsync _dao;
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

        int testEventId = 1; // make sure event exists in test DB
        int testUserId = 1;  // make sure user exists in test DB

        //act
        var result = await registrationDao.JoinEventAsync(testEventId, testUserId);
        //assert
        Assert.That(result, Is.True, "JoinEventAsync should return true when user successfully joins an event.");
    }
}
