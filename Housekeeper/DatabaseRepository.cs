using Housekeeper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Housekeeper
{
    public class DatabaseRepository
    {
        #region Constants

        // Column Constants -- Ends with _COL
        public const string ASSIGNED_COL = "AssignedTo";
        public const string CATEGORY_COL = "Category";
        public const string CHORE_COL = "Chore";
        public const string DURATION_COL = "Duration";
        public const string ID_COL = "ID";
        public const string FREQUENCY_COL = "Frequency";
        public const string NAME_COL = "Username";
        public const string PERFORMED_COL = "LastPerformed";
        public const string TASK_COL = "Task";

        // Table Constants -- Ends with _TABLE
        public const string CHORE_TABLE = "Chore";
        public const string SCHEDULE_TABLE = "Schedule";
        public const string USER_TABLE = "User";

        #endregion Constants

        #region Constructors

        public DatabaseRepository()
        {
            InitializeConnection();
        }

        #endregion Constructors

        #region Properties

        private OleDbConnection _conn { get; set; }

        #endregion Properties

        #region Methods

        private void InitializeConnection()
        {
            string dbPath = Path.Combine(Environment.CurrentDirectory, "Resources\\Housekeeper.mdb");
            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = "Microsoft.Jet.OLEDB.4.0";
            builder.DataSource = dbPath;

            _conn = new OleDbConnection(builder.ConnectionString);
        }

        #region Users

        /// <summary>
        /// Returns a list of all users currently stored in the database.
        /// </summary>
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                _conn.Open();
                string sql = $"SELECT * FROM [{USER_TABLE}]";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.ID = Convert.ToInt32(reader[ID_COL]);
                            user.Username = Convert.ToString(reader[NAME_COL]);
                            users.Add(user);
                        }
                    }
                }
            }
            finally
            {
                _conn.Close();
            }

            return users;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        public void AddUser(string fullName)
        {
            try
            {
                _conn.Open();
                string sql = $"INSERT INTO [{USER_TABLE}] " +
                             $"({NAME_COL}) " +
                             $"VALUES " +
                             $"('{fullName}')";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        #endregion Users

        #region Chores

        /// <summary>
        /// Returns a list of all chores currently stored in the database.
        /// </summary>
        public List<Chore> GetChores()
        {
            List<Chore> chores = new List<Chore>();

            try
            {
                _conn.Open();
                string sql = $"SELECT * FROM [{CHORE_TABLE}]";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Chore chore = new Chore();
                            chore.ID = Convert.ToInt32(reader[ID_COL]);
                            chore.Category = (Chore.ChoreCategory)Enum.Parse(typeof(Chore.ChoreCategory), Convert.ToString(reader[CATEGORY_COL]));
                            chore.Task = Convert.ToString(reader[TASK_COL]);
                            chore.LastPerform = Convert.ToDateTime(reader[PERFORMED_COL]);
                            chore.Frequency = Convert.ToInt32(reader[FREQUENCY_COL]);
                            chore.Duration = reader[DURATION_COL] != DBNull.Value ? Convert.ToInt32(reader[DURATION_COL]) : (int?)null;
                            chores.Add(chore);
                        }
                    }
                }
            }
            finally
            {
                _conn.Close();
            }

            return chores;
        }

        /// <summary>
        /// Adds a new chore to the database
        /// </summary>
        public void AddChore(Chore newChore)
        {
            try
            {
                _conn.Open();
                string sql = $"INSERT INTO [{CHORE_TABLE}] " +
                             $"({CATEGORY_COL}, {TASK_COL}, {FREQUENCY_COL}, {DURATION_COL}, {PERFORMED_COL}) " +
                             $"VALUES " +
                             $"('{newChore.Category.ToString()}', '{newChore.Task}', {newChore.Frequency}, {newChore.Duration ?? 0}, '{DateTime.Today}')";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        /// <summary>
        /// Modifies an existing chore in the database with new, user given values
        /// </summary>
        public void ModifyChore(ScheduledChore modifiedChore)
        {
            try
            {
                _conn.Open();
                string sql = $"UPDATE [{CHORE_TABLE}] " +
                             $"SET {FREQUENCY_COL} = {modifiedChore.Frequency}, " +
                             $"{DURATION_COL} = {modifiedChore.Duration}, " +
                             $"{PERFORMED_COL} = '{modifiedChore.LastPerform}' " +
                             $"WHERE {ID_COL} = {modifiedChore.ChoreID}";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        /// <summary>
        /// Removes the chore from the directory
        /// </summary>
        public void DeleteChore(ScheduledChore chore)
        {
            try
            {
                _conn.Open();

                string sql = $"DELETE FROM [{SCHEDULE_TABLE}] " +
                      $"WHERE {CHORE_COL} = {chore.ChoreID}";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }

                sql = $"DELETE FROM [{CHORE_TABLE}] " +
                      $"WHERE {ID_COL} = {chore.ChoreID}";
                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        #endregion Chores

        #region Schedule

        /// <summary>
        /// Gets a list of chores that have already been scheduled for the household
        /// </summary>
        public ObservableCollection<ScheduledChore> GetScheduledChores(List<Chore> allChores, List<User> allUsers, User currentUser)
        {
            ObservableCollection<ScheduledChore> tasks = new ObservableCollection<ScheduledChore>();

            try
            {
                _conn.Open();
                string sql = $"SELECT * FROM [{SCHEDULE_TABLE}]";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int choreId = Convert.ToInt32(reader[CHORE_COL]);
                            int userId = Convert.ToInt32(reader[ASSIGNED_COL]);
                            Chore chore = allChores.FirstOrDefault(c => c.ID == choreId);
                            User user = allUsers.First(u => u.ID == userId);
                            ScheduledChore scheduledChore = new ScheduledChore(chore)
                            {
                                ID = Convert.ToInt32(reader[ID_COL]),
                                ChoreID = choreId,
                                UserID = userId,
                                AssignedTo = user
                            };

                            tasks.Add(scheduledChore);
                        }
                    }
                }

                List<string> userOrder = new List<string> { currentUser.Username };
                tasks = new ObservableCollection<ScheduledChore>(tasks.OrderBy(c =>
                        {
                            int index = userOrder.IndexOf(c.AssignedTo.Username);
                            return index == -1 ? int.MaxValue : index;
                        }));
            }
            finally
            {
                _conn.Close();
            }

            return tasks;
        }

        /// <summary>
        /// Assigns the chore to the user, updating the assignment if the chore was already scheduled
        /// </summary>
        public void AssignChore(ScheduledChore chore)
        {
            try
            {
                _conn.Open();

                string sql = string.Empty;

                if (chore.ID > 0)
                    sql = $"UPDATE [{SCHEDULE_TABLE}] " +
                             $"SET {ASSIGNED_COL} = {chore.UserID} " +
                             $"WHERE {ID_COL} = {chore.ID}";
                else
                    sql = $"INSERT INTO [{SCHEDULE_TABLE}] " +
                          $"({ASSIGNED_COL}, {CHORE_COL}) " +
                          $"VALUES " +
                          $"({chore.UserID}, {chore.ChoreID})";

                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        /// <summary>
        /// Removes the chore from the current schedule
        /// </summary>
        public void DeleteScheduledChore(ScheduledChore chore)
        {
            try
            {
                _conn.Open();

                string sql = $"DELETE FROM [{SCHEDULE_TABLE}] " +
                             $"WHERE {ID_COL} = {chore.ID}";
                using (OleDbCommand command = new OleDbCommand(sql, _conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _conn.Close();
            }
        }

        #endregion Schedule

        #endregion Methods
    }
}
