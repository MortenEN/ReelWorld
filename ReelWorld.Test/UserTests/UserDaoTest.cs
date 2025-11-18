using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test.UserTests
{
    public class UserDaoTest
    {
        private IUserDaoAsync _dao;
        private const string connectionsString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";

        [SetUp]
        public void Setup()
        {
            _dao = new InMemoryUserDaoStub();
        }

        [Test]
        public void UserDao_Create_User_With_Stub()
        {
            //arrange
            var user = new User { UserId = 0, Name = "Alice" };
            //act
            var createdId = _dao.CreateAsync(user);
            int createdIdInt = createdId.Result;
            //assert
            Assert.That(createdIdInt, Is.EqualTo(1));
            Assert.That(user.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task UserDao_Create_User_With_Database()
        {
            //arrange
            UserDao userDao = new(connectionsString);
            List<string> interests;
            interests = new List<string>();
            interests.Add("paddle");
            User user = new User("Test_UserDao_Create_User_With_Database", "test@testing.eu", "1234", 12345678, "21", 1, interests, "a test", "Aalborg", "Danmark", "Gaden", "12", "9000");
            //act
            int newUserId = await userDao.CreateAsync(user);
            //assert
            Assert.That(newUserId, Is.GreaterThan(0), "The Create method should return a UserId that is above 0");
        }

    }
}