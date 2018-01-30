namespace Housekeeper.Model
{
    public class ScheduledChore : Chore
    {
        #region Properties

        public new int ID { get; set; }
        public int ChoreID { get; set; }
        public int UserID { get; set; }

        public Chore Chore { get; set; }
        public User AssignedTo { get; set; }

        #endregion Properties
    }
}
