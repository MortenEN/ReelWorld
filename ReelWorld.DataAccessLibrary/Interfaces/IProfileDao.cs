using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    /// <summary>
    /// Defines methods for Create, Read, Update and Delete operations on Profile.
    /// </summary>
    public interface IProfileDao
    {
        /// <summary>
        /// Retrieves a single Profile by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the Profile object if found and if a Profile is not found Returns null</returns>
        Profile? GetOne(int id);
        /// <summary>
        /// Retrieves all Profiles from the data source.
        /// </summary>
        /// <returns>Returns an IEnumerable containing all Profile objects.</returns>
        IEnumerable<Profile> GetAll();
        /// <summary>
        /// Creates a new Profile in the data sources.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>The unique identifier (ID) of the newly created Profile.</returns>
        int Create(Profile profile);
        /// <summary>
        /// Updates an existing profile in the data source.
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Returns true if the update was successful; otherwise, returns false.</returns>
        bool Update(Profile profile);
        /// <summary>
        /// Deletes a Profile by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns true if the deletion was successful; otherwise, returns false.</returns>
        bool Delete(int id);
    }
}