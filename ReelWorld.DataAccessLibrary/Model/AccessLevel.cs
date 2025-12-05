using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelWorld.DataAccessLibrary.Model
{
    public class AccessLevel
    {
        #region Properties
        public int AccessLevelId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Constructers
        public AccessLevel(int accessLevelId, string name, string? description)
        {
            AccessLevelId = accessLevelId;
            Name = name;
            Description = description;
        }
        public AccessLevel()
        {

        }
        #endregion
    }
}
