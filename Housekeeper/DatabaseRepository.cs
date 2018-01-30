using Housekeeper.Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Housekeeper
{
    public class DatabaseRepository
    {
        #region Constants

        // Column Constants -- Ends with _COL
        public const string ASSIGNED_COL = "AssignedTo";
        public const string CATEGORY_COL = "Category";
        public const string CHORE_COL = "Chore";
        public const string ID_COL = "ID";
        public const string FREQUENCY_COL = "Frequency";
        public const string NAME_COL = "Username";
        public const string PERFORMED_COL = "LastPerform";
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
            string dbPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\Housekeeper.mdb");
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
                string sql = $"Select * FROM [{USER_TABLE}]";

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
        public void AddUser(int fullName)
        {
            try
            {
                _conn.Open();
                string sql = $"INSERT INTO [{USER_TABLE}] ({NAME_COL}) VALUES ({fullName})";

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

        #endregion Chores

        #region Schedule

        public List<ScheduledChore> GetScheduledChores(List<Chore> allChores)
        {
            List<ScheduledChore> tasks = new List<ScheduledChore>();

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
                            ScheduledChore scheduledChore = new ScheduledChore();
                            scheduledChore.ID = Convert.ToInt32(reader[ID_COL]);
                            scheduledChore.ChoreID = Convert.ToInt32(reader[CHORE_COL]);
                            scheduledChore.UserID = Convert.ToInt32(reader[ASSIGNED_COL]);
                            tasks.Add(scheduledChore);
                        }
                    }
                }
            }
            finally
            {
                _conn.Close();
            }

            return tasks;
        }

        #endregion Schedule

        #endregion Methods
    }
}
