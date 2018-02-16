namespace Housekeeper.Model
{
    public class ScheduledChore : Chore
    {
        #region Constructors

        public ScheduledChore(Chore chore)
        {
            Category = chore.Category;
            Task = chore.Task;
            LastPerform = chore.LastPerform;
            Frequency = chore.Frequency;
            Duration = chore.Duration;
        }

        public ScheduledChore() { }

        #endregion Constructors

        #region Properties

        public new int ID { get; set; }
        public int ChoreID { get; set; }
        public int UserID { get; set; }

        public User AssignedTo { get; set; }

        #endregion Properties
    }
}
