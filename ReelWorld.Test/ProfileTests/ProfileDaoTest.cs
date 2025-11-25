using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;
using ReelWorld.DataAccessLibrary.Stub;

namespace ReelWorld.Test.ProfileTests
{
    public class ProfileDaoTest
    {
        private IProfileDaoAsync _dao;
        private const string connectionsString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";

        [SetUp]
        public void Setup()
        {
            _dao = new InMemoryProfileDaoStub();
        }

        [Test]
        public void ProfileDao_Create_Profile_With_Stub()
        {
            // Arrange
            var interests = new List<string> { "Hiking", "Movies" };
            var profile = new Profile
            {
                Name = "Alice Jensen",
                Email = "alice@test.com",
                HashPassword = "1234",
                PhoneNo = "12345678",
                Age = 25,
                Relationship = Profile.RelationshipStatus.Single,
                Description = "Love hiking and movies.",
                City = "Copenhagen",
                Country = "Denmark",
                StreetName = "Gaden",
                StreetNumber = "1",
                ZipCode = "9000",
                Interests = interests
            };

            // Act
            var createdId = _dao.CreateAsync(profile);
            int createdIdInt = createdId.Result;

            // Assert
            Assert.That(createdIdInt, Is.EqualTo(4));
            Assert.That(profile.ProfileId, Is.EqualTo(4));
        }

        [Test]
        public async Task ProfileDao_Create_Profile_With_Database()
        {
            // Arrange
            ProfileDao profileDao = new(connectionsString);
            List<string> interests = new List<string> { "Paddle", "Reading" };
            Profile profile = new Profile
            {
                Name = "Test_ProfileDao_Create_Profile_With_Database",
                Email = "test@testing.ekwek",
                HashPassword = "1234",
                PhoneNo = "87654321",
                Age = 30,
                Relationship = Profile.RelationshipStatus.Single,
                Description = "A test description",
                City = "Aalborg",
                Country = "Denmark",
                StreetName = "Gaden",
                StreetNumber = "12",
                ZipCode = "9000",
                Interests = interests
            };

            // Act
            int newProfileId = await profileDao.CreateAsync(profile);

            // Assert
            Assert.That(newProfileId, Is.GreaterThan(0), "The Create method should return a ProfileID that is above 0");

            //Cleanup
            bool deleteResult = await profileDao.DeleteAsync(newProfileId);
        }



        [Test]
        public async Task ProfileDao_Delete_Profile_With_Database()
        {
            // Arrange
            ProfileDao profileDao = new(connectionsString);

            List<string> interests = new List<string> { "Paddle", "Reading" };

            Profile profile = new Profile
            {
                Name = "Test_ProfileDao_Create_Profile_With_Database",
                Email = "test@testing.testtest",
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
                Interests = interests
            };

            // Act
            int id = await profileDao.CreateAsync(profile);
            bool deleteResult = await profileDao.DeleteAsync(id);

            // Assert
            Assert.Greater(id, 0, "CreateAsync should return a valid ID");
            Assert.IsTrue(deleteResult, "DeleteAsync should return true for a valid profile");

        }

        [Test]
        public async Task ProfileDao_GetOne_Profile_With_Database()
        {
            // Arrange
            ProfileDao profileDao = new(connectionsString);

            List<string> interests = new List<string> { "Paddle", "Reading", "Running" };

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
                Interests = interests
            };

            // Act
            var id = await profileDao.CreateAsync(profile);
            var retrievedProfile = await profileDao.GetOneAsync(id);

            // Assert
            Assert.Greater(id, 0, "CreateAsync should return a valid ID");
            Assert.IsNotNull(retrievedProfile, "GetOneAsync should return a Profile object");

            // Cleanup
            var deleteResult = await profileDao.DeleteAsync(id);
            Assert.IsTrue(deleteResult, "Profile should be deleted in cleanup");
        }

        [Test]
        public async Task ProfileDao_GetAll_With_Database()
        {
            //arrange
            ProfileDao profileDao = new(connectionsString);
            //act
            var profile = await profileDao.GetAllAsync();
            //assert
            Assert.That(profile, Is.Not.Null, "The GetAll method should return a list of events");
            Assert.That(profile.Count(), Is.GreaterThan(0), "The GetAll method should return at least one event");
        }
        //[Test]
        //public async Task ProfileDao_Update_Profile_With_Database()
        //{
        //    // Arrange
        //    ProfileDao profileDao = new(connectionsString);

        //    List<string> interests = new List<string> { "Paddle", "Reading" };

        //    Profile profile = new Profile
        //    {
        //        Name = "Test_ProfileDao_Update_Profile_With_Database",
        //        Email = "test@testing.testtest",
        //        HashPassword = "12345",
        //        PhoneNo = "12345678",
        //        Age = 26,
        //        Relationship = Profile.RelationshipStatus.Single,
        //        Description = "Test test test",
        //        City = "Aalborg",
        //        Country = "Denmark",
        //        StreetName = "Gaden",
        //        StreetNumber = "17",
        //        ZipCode = "9000",
        //        Interests = interests
        //    };

        //    // Act
        //    var id = profileDao.CreateAsync(profile);
        //    profile.Name = "update";
        //    profile.Email = "update@update.net";
        //    profile.HashPassword = "update";
        //    profile.PhoneNo = "87654321";
        //    profile.Age = 62;
        //    profile.Relationship = Profile.RelationshipStatus.Kompliceret;
        //    profile.Description = "update update";
        //    profile.City = "London";
        //    profile.Country = "England";
        //    profile.StreetName = "Lizie line";
        //    profile.StreetNumber = "1";
        //    profile.ZipCode = "1234";
        //    interests.Remove("Paddle");
        //    interests.Remove("Reading");
        //    interests.Add("Updating");
        //    await profileDao.UpdateAsync(profile);

        //    // Assert
        //    var updated = await profileDao.GetOneAsync(await id);

        //    Assert.That(updated, Is.Not.Null);
        //    Assert.That(updated.Name, Is.EqualTo("update"));
        //    Assert.That(updated.Email, Is.EqualTo("update@update.net"));
        //    Assert.That(updated.HashPassword, Is.EqualTo("update"));
        //    Assert.That(updated.PhoneNo, Is.EqualTo("87654321"));
        //    Assert.That(updated.Age, Is.EqualTo(62));
        //    Assert.That(updated.Relationship, Is.EqualTo(Profile.RelationshipStatus.Kompliceret));
        //    Assert.That(updated.Description, Is.EqualTo("update update"));
        //    Assert.That(updated.City, Is.EqualTo("London"));
        //    Assert.That(updated.Country, Is.EqualTo("England"));
        //    Assert.That(updated.StreetName, Is.EqualTo("Lizie line"));
        //    Assert.That(updated.StreetNumber, Is.EqualTo("1"));
        //    Assert.That(updated.ZipCode, Is.EqualTo("1234"));

        //    // Interests check (order-insensitive)
        //    CollectionAssert.AreEquivalent(
        //        new List<string> { "Updating" },
        //        updated.Interests
        //    );
        //}
    }
}
