using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Stub
{
    public class InMemoryProfileDaoStub : IProfileDaoAsync
    {
        private static List<Profile> _profiles = new List<Profile>();

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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Profile?> GetOneAsync(int profileId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }
    }
}
