using System;

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
        public int? Duration { get; set; }

        #endregion Properties

        #region Enums

        public enum ChoreCategory
        {
            Bedroom,
            Kitchen,
            Living,
            Bathroom,
            Outdoors,
            General
        }

        #endregion Enums
    }
}
