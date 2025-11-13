using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public abstract class Profile
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the Profile.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the Email of the Profile.
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Gets or sets the HashPassword of the Profile.
        /// </summary>
        public string hashPassword { get; set; }
        #endregion

        #region Constructor
        protected Profile(string name, string email, string hashPassword)
        {
            /// <param name="name">The Profile's full name.</param>
            /// <param name="email">The Profile's email address.</param>
            /// <param name="hashPassword">The hashed password of the Profile.</param>
            this.name = name;
            this.email = email;
            this.hashPassword = hashPassword;
        }
        #endregion
    }
}
