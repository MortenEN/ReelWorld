using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    abstract class Profile
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
