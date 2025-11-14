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
        public int UserId { get; set;}
        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// Gets or sets the age of the user.
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Represents the relationship status of a user with 3 given types.
        /// </summary>
        public enum Relationship
        {
            Single,
            Taken,
            complicated,
        }
        public Relationship Relation { get; set; }
        /// <summary>
        /// Gets or sets a list of interests associated with the user.
        /// </summary>
        public List<String> Interests { get; set; }
        /// <summary>
        /// Gets or sets a short personal description written by the user.
        /// </summary>
        public string Description { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        #endregion

        #region Constructor
        public User(string name, string email, string hashPassword, int userId, string phoneNo, int age, List<string> interests, string description, string cityName, string countryName) : base(name, email, hashPassword)
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
            this.Name = name;
            this.Email = email;
            this.HashPassword = hashPassword;
            this.UserId = userId;
            this.PhoneNo = phoneNo;
            this.Age = age;
            this.Interests = interests;
            this.Description = description;
            this.CityName = cityName;
            this.CountryName = countryName;
        }

        public User(): base() 
        {
              
        }
        #endregion
    }
}
