using NUnit.Framework;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test.UserTests
{
    public class UserDaoTest
    {
        private const string ConnectionString = "";
        private InMemoryUserDaoStub _dao;


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserDao_Create()
        {
            //arrange
            //act
            //assert
            Assert.Pass();
        }
    }
}