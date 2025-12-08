using Dapper;
using Microsoft.Data.SqlClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.Tools;
using System.Security.Cryptography;
using System.Text;

namespace ReelWorld.DataAccessLibrary.SqlServer
{
    public class LoginDao : BaseDao, ILoginDao
    {
        protected readonly string _connectionString;

        public LoginDao(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Profile?> GetOneAsync(int profileId)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            try
            {
                var query = @"
                            SELECT 
                        p.ProfileID AS ProfileId,
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
                        p.FK_AccessLevel_Id,
                
                        c.City AS City,
                        co.Country AS Country,
                
                        al.AccessLevelId,
                        al.Name,
                        al.Description,
                
                        STRING_AGG(i.InterestName, ', ') AS Interests
                    FROM Profile p
                    LEFT JOIN City c ON p.FK_City_ID = c.CityID
                    LEFT JOIN Country co ON c.FK_Country_ID = co.CountryID
                    LEFT JOIN AccessLevel al ON p.FK_AccessLevel_Id = al.AccessLevelId
                    LEFT JOIN ProfileInterests pi ON p.ProfileID = pi.ProfileID
                    LEFT JOIN Interests i ON pi.InterestsID = i.InterestsID
                    WHERE p.ProfileID = @ProfileId
                    GROUP BY 
                        p.ProfileID, p.Email, p.HashPassword, p.Salt, p.ProfileType,
                        p.FirstName, p.MiddleName, p.Surname, p.PhoneNo, p.Age,
                        p.Relationship, p.Description, p.StreetName, p.StreetNumber, p.ZipCode,
                        p.FK_AccessLevel_Id, c.City, co.Country, al.AccessLevelId, al.Name, al.Description;
                ";

                var result = await connection.QueryAsync<Profile, AccessLevel, Profile>(
                    query,
                    (profile, accessLevel) =>
                    {
                        profile.AccessLevel = accessLevel;
                        return profile;
                    },
                    new { ProfileId = profileId },
                    splitOn: "AccessLevelId"
                );

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Database error in GetOneAsync({profileId})", ex);
            }
        }

        public async Task<int> LoginAsync(string email, string password)
        {
            try
            {
                var query = "SELECT ProfileID, HashPassword FROM Profile WHERE Email=@Email";
                using var connection = (SqlConnection)CreateConnection();
                await connection.OpenAsync();

                var profileTuple = await connection.QueryFirstOrDefaultAsync<ProfileTuple>(query, new { Email = email });
                if (profileTuple != null && BCryptTool.ValidatePassword(password, profileTuple.HashPassword))
                {
                    return profileTuple.ProfileID;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in for author with email {email}: '{ex.Message}'.", ex);
            }
        }

        internal class ProfileTuple
        {
            public int ProfileID { get; set; }
            public string HashPassword { get; set; }
        }
    }
}
