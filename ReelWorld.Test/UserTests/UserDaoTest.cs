using NUnit.Framework;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test.UserTests
{
    public class UserDaoTest
    {
        private IUserDao _dao;


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
            var createdId = _dao.Create(user);
            //assert
            Assert.That(createdId, Is.EqualTo(1));
            Assert.That(user.UserId, Is.EqualTo(1));
        }
    }
}