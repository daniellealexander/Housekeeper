using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Housekeeper.Model
{
    public class Chore
    {
        #region Properties

        public int ID { get; set; }
        public ChoreCategory Category { get; set; }
        public string Task { get; set; }
        public DateTime LastPerform { get; set; }
        public int Frequency { get; set; }

        #endregion Properties

        #region Enums

        public enum ChoreCategory
        {
            General,
            Bedroom,
            Bathroom,
            Kitchen
        }

        #endregion Enums
    }
}
