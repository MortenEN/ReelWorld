using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class User : Profile
    {
        /// <summary>
        /// Represents an application user that inherits common profile information.
        /// </summary>
        /// <remarks>
        /// The <see cref="User"/> class extends the <see cref="Profile"/> class by adding 
        /// user-specific attributes such as phone number, age, interests, and a description.
        /// </remarks>
        #region Properties
        /// <summary>
        /// Gets the unique user identifier.
        /// </summary>
        public int userId { get; }
        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string phoneNo { get; set; }
        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int age { get; set; }
        /// <summary>
        /// Represents the relationship status of a user with 3 given types.
        /// </summary>
        public enum realtionship
        {
            Single,
            Taken,
            complicated,
        }
        /// <summary>
        /// Gets or sets a list of interests associated with the user.
        /// </summary>
        public List<String> Interests { get; set; }
        /// <summary>
        /// Gets or sets a short personal description written by the user.
        /// </summary>
        public string description { get; set; }
        #endregion

        #region Constructor
        public User(string name, string email, string hashPassword, int userId, string phoneNo, int age, List<string> interests, string description):base(name,email,hashPassword)
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="User"/> class with profile and user details.
            /// </summary>
            /// <param name="name">The user's full name.</param>
            /// <param name="email">The user's email address.</param>
            /// <param name="hashPassword">The hashed password of the user.</param>
            /// <param name="userId">The unique identifier for the user.</param>
            /// <param name="phoneNo">The user's phone number.</param>
            /// <param name="age">The user's age.</param>
            /// <param name="interests">A list of user interests.</param>
            /// <param name="description">A brief personal description.</param>
            this.name = name;
            this.email = email;
            this.hashPassword = hashPassword;
            this.userId = userId;
            this.phoneNo = phoneNo;
            this.age = age;
            Interests = interests;
            this.description = description;
        }
        #endregion
    }
}
