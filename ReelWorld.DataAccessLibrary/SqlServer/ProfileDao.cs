using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Tools;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class ProfileDao : BaseDao, IProfileDaoAsync
    {
        public ProfileDao(string connectionString) : base(connectionString)
        {

        }

        public async Task<int> CreateAsync(Profile profile)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Split name → FirstName, MiddleName, Surname
                var (firstName, middleName, surname) = SplitFullName(profile.Name);

                // 2. Find or create Country
                var countryId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CountryID FROM Country WHERE Country = @CountryName;
                ", new { CountryName = profile.CountryName }, transaction);

                if (countryId == null)
                {
                    countryId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO Country (Country) OUTPUT INSERTED.CountryID VALUES (@CountryName);
                    ", new { CountryName = profile.CountryName }, transaction);
                }

                // 3. Find or create City
                var cityId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CityID FROM City WHERE City = @CityName AND FK_Country_ID = @CountryID;
                ", new { CityName = profile.CityName, CountryID = countryId.Value }, transaction);

                if (cityId == null)
                {
                    cityId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO City (City, FK_Country_ID) OUTPUT INSERTED.CityID VALUES (@CityName, @CountryID);
                    ", new { CityName = profile.CityName, CountryID = countryId.Value }, transaction);
                }

                // 4. Insert Profile
                var salt = BCryptTool.GetRandomSalt();
                var passwordHash = BCryptTool.HashPassword(profile.HashPassword, salt);

                var profileQuery = @"
                INSERT INTO Profile 
                (Email, HashPassword, Salt, ProfileType, FirstName, MiddleName, Surname, PhoneNo, Age, Relationship, Description, FK_City_ID, StreetName, StreetNumber, ZipCode)
                OUTPUT INSERTED.ProfileID
                VALUES
                (@Email, @HashPassword, @Salt, @ProfileType, @FirstName, @MiddleName, @Surname, @PhoneNo, @Age, @Relationship, @Description, @CityID, @StreetName, @StreetNumber, @ZipCode);
                ";

                var profileId = await connection.QuerySingleAsync<int>(profileQuery, new
                {
                    Email = profile.Email,
                    HashPassword = passwordHash,
                    Salt = salt,
                    ProfileType = "User",
                    FirstName = firstName,
                    MiddleName = middleName,
                    Surname = surname,
                    PhoneNo = profile.PhoneNo,
                    Age = profile.Age,
                    Relationship = profile.Relationship.ToString(),
                    Description = profile.Description,
                    CityID = cityId.Value,
                    StreetName = profile.StreetName,
                    StreetNumber = profile.StreetNumber,
                    ZipCode = profile.ZipCode
                }, transaction);

                // 5. Insert ProfileInterests
                if (profile.Interests != null)
                {
                    foreach (var interestName in profile.Interests)
                    {
                        var interestId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                        SELECT InterestsID FROM Interests WHERE InterestName = @Name;
                        ", new { Name = interestName }, transaction);

                        if (interestId == null)
                        {
                            interestId = await connection.QuerySingleAsync<int>(@"
                            INSERT INTO Interests (InterestName) OUTPUT INSERTED.InterestsID VALUES (@Name);
                            ", new { Name = interestName }, transaction);
                        }

                        await connection.ExecuteAsync(@"
                        INSERT INTO ProfileInterests (ProfileID, InterestsID)
                        VALUES (@ProfileID, @InterestID);
                        ", new { ProfileID = profileId, InterestID = interestId.Value }, transaction);
                     }
                }

                transaction.Commit();
                return profileId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public Task<Profile?> GetOneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Profile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Profile profile)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "DELETE FROM [Profile] WHERE ProfileID = @ProfileID";
                var affectedRows = await connection.ExecuteAsync(query, new { ProfileID = id });
                return affectedRows > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static (string FirstName, string MiddleName, string Surname) SplitFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return ("", null, "");

            var names = fullName.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            string firstName = names.Length > 0 ? names[0] : "";
            string surname = names.Length > 1 ? names[^1] : "";
            string middleName = names.Length > 2 ? string.Join(' ', names[1..^1]) : null;

            return (firstName, middleName, surname);
        }
    }
}
