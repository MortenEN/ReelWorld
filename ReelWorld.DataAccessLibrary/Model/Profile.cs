using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public abstract class Profile
    {
        public string name { get; set; }
        public string email { get; set; }
        public string hashPassword { get; set; }

        protected Profile(string name, string email, string hashPassword)
        {
            this.name = name;
            this.email = email;
            this.hashPassword = hashPassword;
        }
    }
}
