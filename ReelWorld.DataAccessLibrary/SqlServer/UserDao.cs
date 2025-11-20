using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Tools;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class UserDao : BaseDao, IUserDaoAsync
    {
        public UserDao(string connectionString) : base(connectionString)
        {

        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Split name → FirstName, MiddleName, Surname
                var (firstName, middleName, surname) = SplitFullName(user.Name);

                // 2. Find or create Country
                var countryId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CountryID FROM Country WHERE Country = @CountryName;
                ", new { CountryName = user.CountryName }, transaction);

                if (countryId == null)
                {
                    countryId = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Country (Country) OUTPUT INSERTED.CountryID VALUES (@CountryName);
                ", new { CountryName = user.CountryName }, transaction);
                }

                // 3. Find or create City
                var cityId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CityID FROM City WHERE City = @CityName AND FK_Country_ID = @CountryID;
                ", new { CityName = user.CityName, CountryID = countryId.Value }, transaction);

                if (cityId == null)
                {
                    cityId = await connection.QuerySingleAsync<int>(@"
                INSERT INTO City (City, FK_Country_ID) OUTPUT INSERTED.CityID VALUES (@CityName, @CountryID);
                ", new { CityName = user.CityName, CountryID = countryId.Value }, transaction);
                }

                // 4. Insert Profile
                var profileQuery = @"
                INSERT INTO Profile (Email, HashPassword, Salt, ProfileType, FirstName, MiddleName, Surname)
                OUTPUT INSERTED.ProfileID
                VALUES (@Email, @HashPassword, @Salt, @ProfileType, @FirstName, @MiddleName, @Surname);
                ";
                var salt = BCryptTool.GetRandomSalt();
                var passwordHash = BCryptTool.HashPassword(user.HashPassword, salt);
                var profileId = await connection.QuerySingleAsync<int>(profileQuery, new
                {
                    Email = user.Email,
                    HashPassword = passwordHash,
                    Salt = salt,
                    ProfileType = "User",
                    FirstName = firstName,
                    MiddleName = middleName,
                    Surname = surname
                }, transaction);

                // 5. Insert User
                var userQuery = @"
                INSERT INTO [User] (Phoneno, Age, RelationShip, Description, FK_Profile_ID, FK_City_ID, StreetName, StreetNumber, ZipCode)
                OUTPUT INSERTED.UserID
                VALUES (@PhoneNo, @Age, @Relationship, @Description, @ProfileID, @CityID, @StreetName, @StreetNumber, @ZipCode);
                ";
                var userId = await connection.QuerySingleAsync<int>(userQuery, new
                {
                    PhoneNo = user.PhoneNo,
                    Age = user.Age,
                    Relationship = user.Relation.ToString(),
                    Description = user.Description,
                    ProfileID = profileId,
                    CityID = cityId.Value,
                    StreetName = user.StreetName,
                    StreetNumber = user.StreetNumber,
                    ZipCode = user.ZipCode,
                }, transaction);

                // 6. Insert UserInterests
                if (user.Interests != null)
                {
                    foreach (var interestName in user.Interests)
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
                    INSERT INTO UserInterests (UserID, InterestsID)
                    VALUES (@UserID, @InterestID);
                    ", new { UserID = userId, InterestID = interestId.Value }, transaction);
                    }
                }

                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<User?> GetOneAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = "DELETE FROM [User] WHERE UserID = @UserID";
                var affectedRows = await connection.ExecuteAsync(query, new { UserID = userId });
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
