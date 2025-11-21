using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMemoryProfileDaoStub : IProfileDaoAsync
    {
        private static List<Profile> _profiles = new List<Profile>();
        public InMemoryProfileDaoStub()
        {
            if (_profiles.Count == 0)
            {
                _profiles.Add(new Profile
                {
                    ProfileId = 1,
                    Name = "Anna Jensen",
                    Email = "anna@test.com",
                    HashPassword = "HASH1",
                    PhoneNo = "12345678",
                    Age = 25,
                    Relationship = Profile.RelationshipStatus.Single,
                    Description = "Love hiking and movies.",
                    CityName = "Aalborg",
                    CountryName = "Danmark",
                    StreetName = "Gaden",
                    StreetNumber = "1",
                    ZipCode = "9000",
                    Interests = new List<string> { "Hiking", "Movies" }
                });

                _profiles.Add(new Profile
                {
                    ProfileId = 2,
                    Name = "Mikkel Hansen",
                    Email = "mikkel@test.com",
                    HashPassword = "HASH2",
                    PhoneNo = "22334455",
                    Age = 31,
                    Relationship = Profile.RelationshipStatus.Single,
                    Description = "Gamer and reader.",
                    CityName = "Aalborg",
                    CountryName = "Danmark",
                    StreetName = "Gaden",
                    StreetNumber = "1",
                    ZipCode = "9000",
                    Interests = new List<string> { "Gaming", "Reading" }
                });

                _profiles.Add(new Profile
                {
                    ProfileId = 3,
                    Name = "Sara Olsen",
                    Email = "sara@test.com",
                    HashPassword = "HASH3",
                    PhoneNo = "33445566",
                    Age = 28,
                    Relationship = Profile.RelationshipStatus.Kompliceret,
                    Description = "Enjoys cooking and fitness.",
                    CityName = "Aalborg",
                    CountryName = "Danmark",
                    StreetName = "Gaden",
                    StreetNumber = "1",
                    ZipCode = "9000",
                    Interests = new List<string> { "Cooking", "Fitness" }
                });
            }
        }
        public Task<int> CreateAsync(Profile profile)
        {
            var newId = 1;
            if (_profiles.Any())
            {
                newId = _profiles.Max(p => p.ProfileId) + 1;
            }
            profile.ProfileId = newId;
            _profiles.Add(profile);
            return Task.FromResult(newId);
        }

        public Task<bool> DeleteAsync(int profileId)
        {
            var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
            if (profile != null)
            {
                _profiles.Remove(profile);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Profile>> GetAllAsync()
        {
            return Task.FromResult(_profiles.AsEnumerable());
        }

        public Task<Profile?> GetOneAsync(int profileId)
        {
            var profile = _profiles.FirstOrDefault(p => p.ProfileId == profileId);
            return Task.FromResult(profile);
        }

        public Task<bool> UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
