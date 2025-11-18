using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.DataAccessLibrary.Interfaces
{
    /// <summary>
    /// Defines methods for Create, Read, Update and Delete operations on User.
    /// </summary>
    public interface IUserDao
    {
        /// <summary>
        /// Retrieves a single User by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the User object if found and if a User is not found Returns null</returns>
        User? GetOne(int id);
        /// <summary>
        /// Retrieves all users from the data source.
        /// </summary>
        /// <returns>Returns an IEnumerable containing all user objects.</returns>
        IEnumerable<User> GetAll();
        /// <summary>
        /// Creates a new user in the data sources.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The unique identifier (ID) of the newly created user.</returns>
        int Create(User user);
        /// <summary>
        /// Updates an existing user in the data source.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the update was successful; otherwise, returns false.</returns>
        bool Update(User user);
        /// <summary>
        /// Deletes a user by its unique identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns true if the deletion was successful; otherwise, returns false.</returns>
        bool Delete(int id);
    }
}