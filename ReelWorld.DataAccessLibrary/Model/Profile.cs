using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class Profile
    {
        #region Properties
        public int ProfileId { get; set; }
        /// <summary>
        /// Gets or sets the name of the Profile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Email of the Profile.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the HashPassword of the Profile.
        /// </summary>
        public string HashPassword { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the Profile.
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Gets or sets the age of the Profile.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Represents the relationship status of a Profile.
        /// </summary>
        public enum RelationshipStatus
        {
            Single,
            Optaget,
            Kompliceret
        }
        public RelationshipStatus Relation { get; set; }

        /// <summary>
        /// Gets or sets a list of interests associated with the Profile.
        /// </summary>
        public List<string> Interests { get; set; }

        /// <summary>
        /// Gets or sets a short personal description written by the Profile.
        /// </summary>
        public string Description { get; set; }

        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class with full details.
        /// </summary>
        public Profile(int profileId, string name, string email, string hashPassword, string phoneNo, int age, List<string> interests, string description, string cityName, string countryName, string streetName, string streetNumber, string zipCode, RelationshipStatus relation)
        {
            this.ProfileId = profileId;
            this.Name = name;
            this.Email = email;
            this.HashPassword = hashPassword;
            this.PhoneNo = phoneNo;
            this.Age = age;
            this.Interests = interests;
            this.Description = description;
            this.CityName = cityName;
            this.CountryName = countryName;
            this.StreetName = streetName;
            this.StreetNumber = streetNumber;
            this.ZipCode = zipCode;
            this.Relation = relation;
        }

        /// <summary>
        /// Parameterless constructor for initialization without parameters.
        /// </summary>
        public Profile()
        {
        }
        #endregion
    }
}
