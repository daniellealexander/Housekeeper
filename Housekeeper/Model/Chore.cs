using System;
using System.ComponentModel;

namespace Housekeeper.Model
{
    public class Chore
    {
        #region Properties

        public int ID { get; set; }
        public ChoreCategory Category { get; set; }
        public string CategoryString { get { return Category.ToString(); } }
        public string Task { get; set; }
        public DateTime LastPerform { get; set; }
        public int Frequency { get; set; }
        public int? Duration { get; set; }

        #endregion Properties

        #region Enums

        public enum ChoreCategory
        {
            [Description("Bedroom")]
            Bedroom,

            [Description("Kitchen")]
            Kitchen,

            [Description("Living")]
            Living,

            [Description("Bathroom")]
            Bathroom,

            [Description("Outdoors")]
            Outdoors,

            [Description("General")]
            General
        }

        #endregion Enums

        public override string ToString()
        {
            return $"{Category} - {Task}";
        }
    }
}
