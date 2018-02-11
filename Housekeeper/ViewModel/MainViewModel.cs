using System;
using Housekeeper.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Housekeeper.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Member Variables

        public event PropertyChangedEventHandler PropertyChanged;

        private DatabaseRepository _repo { get; set; }

        #endregion Member Variables

        #region Constructors

        public MainViewModel()
        {
            _repo = new DatabaseRepository();
            InitializeCollections();

            ShowLogin = true;
        }

        #endregion Constructors

        #region Properties

        public List<User> AllUsers { get; set; }
        public List<Chore> AllChores { get; set; }
        public List<Task> AllTasks { get; set; }
        public List<ScheduledChore> ScheduledChores { get; set; }

        public User CurrentUser { get; set; }
        public DataTable Schedule { get; set; }
        public ScheduledChore SelectedChore { get; set; }

        public bool ShowLogin { get; set; }
        public bool AllowLogin { get { return CurrentUser != null; } }
        public bool LoggedIn { get { return !ShowLogin; } }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Queries the database to initialize the chore and user collections
        /// </summary>
        private void InitializeCollections()
        {
            AllUsers = _repo.GetUsers();
            AllChores = _repo.GetChores();
            ScheduledChores = _repo.GetScheduledChores();

            InterpolateSchedule();
        }

        /// <summary>
        /// Interprets user and chore IDs into human-readable names
        /// </summary>
        private void InterpolateSchedule()
        {
            foreach (ScheduledChore chore in ScheduledChores)
            {
                chore.Chore = AllChores.FirstOrDefault(c => c.ID == chore.ChoreID);
                chore.AssignedTo = AllUsers.FirstOrDefault(u => u.ID == chore.UserID);
            }

            OnPropertyChanged("ScheduledChores");
        }

        /// <summary>
        /// Updates the boolean properties for state
        /// </summary>
        public void Login()
        {
            if (CurrentUser == null) return;

            ShowLogin = false;
            OnPropertyChanged("ShowLogin");
            OnPropertyChanged("LoggedIn");
        }

        /// <summary>
        /// Requests the user be added to the database, then re-queries the local collection
        /// </summary>
        public void AddUser(string userName)
        {
            _repo.AddUser(userName);
            AllUsers = _repo.GetUsers();
            OnPropertyChanged("AllUsers");
        }

        /// <summary>
        /// Requests the chore be added to the database, then re-queries the local collection
        /// </summary>
        public void AddChore(Chore newChore)
        {
            _repo.AddChore(newChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");
        }

        /// <summary>
        /// Requests the chore be editted in the database, then re-queries the local collection
        /// </summary>
        public void EditChore(Chore modifiedChore)
        {
            _repo.ModifyChore(modifiedChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");
        }

        /// <summary>
        /// Requests the specified chore be assigned to the specified user, then re-queries the local collection
        /// </summary>
        public void ScheduleChore(Chore chore, User user)
        {
            ScheduledChore scheduledChore = chore as ScheduledChore;
            _repo.AssignChore(chore, user, scheduledChore?.ID ?? -1);
            ScheduledChores = _repo.GetScheduledChores();

            InterpolateSchedule();
        }

        /// <summary>
        /// Updates the last performed date to today and removes the selected chore from the schedule
        /// </summary>
        public void CompleteChore()
        {
            SelectedChore.LastPerform = DateTime.Today;
            _repo.ModifyChore(SelectedChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");

            _repo.CompleteChore(SelectedChore);
        }

        /// <summary>
        /// Helper method to update the enabled state for the Login button
        /// </summary>
        public void UpdateProperties()
        {
            OnPropertyChanged("AllowLogin");
        }

        /// <summary>
        /// Required for IPropertyChanged
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
