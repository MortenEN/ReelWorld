using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
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
                SELECT CountryID FROM Country WHERE Country = @Country;
                ", new { Country = profile.Country }, transaction);

                if (countryId == null)
                {
                    countryId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO Country (Country) OUTPUT INSERTED.CountryID VALUES (@Country);
                    ", new { Country = profile.Country }, transaction);
                }

                // 3. Find or create City
                var cityId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CityID FROM City WHERE City = @City AND FK_Country_ID = @CountryID;
                ", new { City = profile.City, CountryID = countryId.Value }, transaction);

                if (cityId == null)
                {
                    cityId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO City (City, FK_Country_ID) OUTPUT INSERTED.CityID VALUES (@City, @CountryID);
                    ", new { City = profile.City, CountryID = countryId.Value }, transaction);
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
                if (!string.IsNullOrWhiteSpace(profile.Interests))
                {
                    // Del strengen op i enkelte interesser, fx "Paddle, Reading"
                    var interests = profile.Interests
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Distinct(StringComparer.OrdinalIgnoreCase); // undgå dubletter som "Paddle" og "paddle"

                    foreach (var interestName in interests)
                    {
                        var interestId = await connection.QuerySingleOrDefaultAsync<int?>(@"
            SELECT InterestsID 
            FROM Interests 
            WHERE InterestName = @Name;
        ", new { Name = interestName }, transaction);

                        if (interestId == null)
                        {
                            interestId = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Interests (InterestName) 
                OUTPUT INSERTED.InterestsID 
                VALUES (@Name);
            ", new { Name = interestName }, transaction);
                        }

                        // 🔒 Indsæt kun, hvis kombinationen ikke allerede findes
                        await connection.ExecuteAsync(@"
            IF NOT EXISTS (
                SELECT 1 
                FROM ProfileInterests 
                WHERE ProfileID = @ProfileID 
                  AND InterestsID = @InterestID
            )
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


        public async Task<Profile?> GetOneAsync(int profileId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = @"
                SELECT 
                p.ProfileID      AS ProfileId,
                p.Email,
                p.HashPassword,
                p.Salt,
                p.ProfileType,
                LTRIM(RTRIM(CONCAT(p.FirstName, ' ', ISNULL(p.MiddleName + ' ', ''), p.Surname))) AS Name,
                p.PhoneNo,
                p.Age,
                p.Relationship,
                p.Description,
                p.StreetName,
                p.StreetNumber,
                p.ZipCode,

                c.City      AS City,
                co.Country  AS Country,

                -- Hent alle interesser som én kommasepareret streng
                STRING_AGG(i.InterestName, ', ') AS Interests

                FROM Profile p
                LEFT JOIN City c               ON p.FK_City_ID = c.CityID
                LEFT JOIN Country co           ON c.FK_Country_ID = co.CountryID
                LEFT JOIN ProfileInterests pi  ON p.ProfileID = pi.ProfileID
                LEFT JOIN Interests i          ON pi.InterestsID = i.InterestsID

                WHERE p.ProfileID = @ProfileId
                GROUP BY 
                    p.ProfileID,
                    p.Email,
                    p.HashPassword,
                    p.Salt,
                    p.ProfileType,
                    p.FirstName,
                    p.MiddleName,
                    p.Surname,
                    p.PhoneNo,
                    p.Age,
                    p.Relationship,
                    p.Description,
                    p.StreetName,
                    p.StreetNumber,
                    p.ZipCode,
                    c.City,
                    co.Country;
                ";
                var result = await connection.QuerySingleOrDefaultAsync<Profile>(query, new { ProfileId = profileId });
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Database error in GetOneAsync({profileId})", ex);
            }
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            var query = @"
                SELECT 
                p.ProfileID      AS ProfileId,
                p.Email,
                p.HashPassword,
                p.Salt,
                p.ProfileType,
                LTRIM(RTRIM(CONCAT(p.FirstName, ' ', ISNULL(p.MiddleName + ' ', ''), p.Surname))) AS Name,
                p.PhoneNo,
                p.Age,
                p.Relationship,
                p.Description,
                p.StreetName,
                p.StreetNumber,
                p.ZipCode,

                c.City      AS City,
                co.Country  AS Country,

                -- Hent alle interesser som én kommasepareret streng
                STRING_AGG(i.InterestName, ', ') AS Interests

                FROM Profile p
                LEFT JOIN City c               ON p.FK_City_ID = c.CityID
                LEFT JOIN Country co           ON c.FK_Country_ID = co.CountryID
                LEFT JOIN ProfileInterests pi  ON p.ProfileID = pi.ProfileID
                LEFT JOIN Interests i          ON pi.InterestsID = i.InterestsID

                GROUP BY 
                    p.ProfileID,
                    p.Email,
                    p.HashPassword,
                    p.Salt,
                    p.ProfileType,
                    p.FirstName,
                    p.MiddleName,
                    p.Surname,
                    p.PhoneNo,
                    p.Age,
                    p.Relationship,
                    p.Description,
                    p.StreetName,
                    p.StreetNumber,
                    p.ZipCode,
                    c.City,
                    co.Country;
                ";
            using var connection = CreateConnection();
            return await connection.QueryAsync<Profile>(query);
        }

        public async Task<bool> UpdateAsync(Profile profile)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 0. Check if profile exists
                var exists = await connection.QuerySingleAsync<int>(@"
                SELECT COUNT(*) FROM Profile WHERE ProfileID = @ProfileID;",
                new { ProfileID = profile.ProfileId }, transaction);

                if (exists == 0)
                {
                    transaction.Rollback();
                    return false; // Profile does not exist → stop here
                }

                // 1. Split full name
                var (firstName, middleName, surname) = SplitFullName(profile.Name);

                // 2. Country lookup/create
                var countryId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CountryID FROM Country WHERE Country = @Country;",
                new { Country = profile.Country }, transaction);

                if (countryId == null)
                {
                    countryId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO Country (Country) 
                    OUTPUT INSERTED.CountryID 
                    VALUES (@Country);",
                    new { Country = profile.Country }, transaction);
                }

                // 3. City lookup/create
                var cityId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                SELECT CityID FROM City WHERE City = @City AND FK_Country_ID = @CountryID;",
                new { City = profile.City, CountryID = countryId.Value }, transaction);

                if (cityId == null)
                {
                    cityId = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO City (City, FK_Country_ID) 
                    OUTPUT INSERTED.CityID 
                    VALUES (@City, @CountryID);",
                    new { City = profile.City, CountryID = countryId.Value }, transaction);
                }

                // 4. Update profile
                await connection.ExecuteAsync(@"
                UPDATE Profile SET
                Email = @Email,
                FirstName = @FirstName,
                MiddleName = @MiddleName,
                Surname = @Surname,
                PhoneNo = @PhoneNo,
                Age = @Age,
                Relationship = @Relationship,
                Description = @Description,
                FK_City_ID = @CityID,
                StreetName = @StreetName,
                StreetNumber = @StreetNumber,
                ZipCode = @ZipCode
                WHERE ProfileID = @ProfileID;",

                new
                {
                    Email = profile.Email,
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
                    ZipCode = profile.ZipCode,
                    ProfileID = profile.ProfileId
                }, transaction);

                // 5. Replace interests
                await connection.ExecuteAsync(@"
                DELETE FROM ProfileInterests WHERE ProfileID = @ProfileID;",
                new { ProfileID = profile.ProfileId }, transaction);

                if (!string.IsNullOrWhiteSpace(profile.Interests))
                {
                    var interests = profile.Interests
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Distinct(StringComparer.OrdinalIgnoreCase);

                    foreach (var interestName in interests)
                    {
                        var interestId = await connection.QuerySingleOrDefaultAsync<int?>(@"
                        SELECT InterestsID FROM Interests WHERE InterestName = @Name;",
                            new { Name = interestName }, transaction);

                        if (interestId == null)
                        {
                            interestId = await connection.QuerySingleAsync<int>(@"
                            INSERT INTO Interests (InterestName) 
                            OUTPUT INSERTED.InterestsID 
                            VALUES (@Name);",
                                new { Name = interestName }, transaction);
                        }

                        await connection.ExecuteAsync(@"
                        INSERT INTO ProfileInterests (ProfileID, InterestsID)
                        VALUES (@ProfileID, @InterestID)",
                            new { ProfileID = profile.ProfileId, InterestID = interestId.Value }, transaction);
                    }
                }

                // SUCCESS
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
            catch (Exception ex)
            {
                throw new Exception($"Database error while deleting profile {id}.", ex);
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

        public async Task<int> LoginAsync(string email, string password)
        {
            try
            {
                var query = "SELECT Id, PasswordHash FROM Profile WHERE Email=@Email";
                using var connection = CreateConnection();

                var profileTuple = await connection.QueryFirstOrDefaultAsync<ProfileTuple>(query, new { Email = email });
                if (profileTuple != null && BCryptTool.ValidatePassword(password, profileTuple.PasswordHash))
                {
                    return profileTuple.Id;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in for profile with email {email}: '{ex.Message}'.", ex);
            }
        }
        internal class ProfileTuple
        {
            public int Id;
            public string PasswordHash;
        }
    }
}
