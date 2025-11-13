using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class User : Profile
    {
        public int userId { get; set; }
        public string phoneNo { get; set; }
        public int age { get; set; }

        public enum realtionship
        {
            Single,
            Taken,
            complicated,
        }
        public List<String> Interests { get; set; }
        public string description { get; set; }

        public User(string name, string email, string hashPassword, int userId, string phoneNo, int age, List<string> interests, string description):base(name,email,hashPassword)
        {
            this.name = name;
            this.email = email;
            this.hashPassword = hashPassword;
            this.userId = userId;
            this.phoneNo = phoneNo;
            this.age = age;
            Interests = interests;
            this.description = description;
        }
    }
}
