using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Test.LoginTests
{
    public class LoginDaoTest
    {
        private ILoginDao _loginDao;
        private const string connectionsString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";


        [Test]
        public async Task LoginDao_LoginAsync_With_Database()
        {
            // Arrange
            LoginDao loginDao = new(connectionsString);
            ProfileDao profileDao = new(connectionsString);
            var profile = new Profile
            {
                Name = "Test_ProfileDao_GetOne_Profile_With_Database",
                Email = "test_profiledao@example.ke",
                HashPassword = "12345",
                PhoneNo = "12345678",
                Age = 26,
                Relationship = Profile.RelationshipStatus.Single,
                Description = "Test test test",
                City = "Aalborg",
                Country = "Denmark",
                StreetName = "Gaden",
                StreetNumber = "17",
                ZipCode = "9000",
                Interests = "Paddle, Football"
            };

            // Act
            int profileId = await profileDao.CreateAsync(profile);
            int loginId = await loginDao.LoginAsync("test_profiledao@example.ke", "12345");
            // Assert
            Assert.That(loginId > 0, "If the check passes, the profile is successfully logged in.");

            // Cleanup
            bool deleteResult = await profileDao.DeleteAsync(profileId);
            Assert.IsTrue(deleteResult, "Profile should be deleted in cleanup");
        }
    }
}
