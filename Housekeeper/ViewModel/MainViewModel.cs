using Housekeeper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

        public List<string> AllCategories { get; set; }
        public List<User> AllUsers { get; set; }
        public List<Chore> AllChores { get; set; }
        public List<Chore> CategorizedChores { get; set; }
        public List<Task> AllTasks { get; set; }
        public ObservableCollection<ScheduledChore> ScheduledChores { get; set; }

        public User CurrentUser { get; set; }
        public DataTable Schedule { get; set; }
        public ScheduledChore SelectedChore { get; set; }

        public bool ShowLogin { get; set; }
        public bool AllowLogin { get { return CurrentUser != null; } }
        public bool LoggedIn { get { return !ShowLogin; } }

        public bool EdittingChore { get; set; }
        public bool AddingChore { get; set; }
        public bool ChoreSelected { get { return SelectedChore != null; } }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Queries the database to initialize the chore and user collections
        /// </summary>
        private void InitializeCollections()
        {
            AllCategories = new List<string>() { "Bedroom", "Kitchen", "Living", "Bathroom", "Outdoors", "General" };
            AllUsers = _repo.GetUsers();
            AllChores = _repo.GetChores();
        }

        /// <summary>
        /// Updates the boolean properties for state
        /// </summary>
        public void Login()
        {
            if (CurrentUser == null) return;

            ScheduledChores = _repo.GetScheduledChores(AllChores, AllUsers, CurrentUser);
            OnPropertyChanged("ScheduledChores");

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
        public void AddChore()
        {
            _repo.AddChore(SelectedChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");
        }

        /// <summary>
        /// Requests the chore be editted in the database, then re-queries the local collection
        /// </summary>
        public void EditChore()
        {
            _repo.ModifyChore(SelectedChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");
        }

        /// <summary>
        /// Requests the specified chore be assigned to the specified user, then re-queries the local collection
        /// </summary>
        public void ScheduleChore()
        {
            _repo.AssignChore(SelectedChore);
            ScheduledChores = _repo.GetScheduledChores(AllChores, AllUsers, CurrentUser);
            OnPropertyChanged("ScheduledChores");
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

            _repo.DeleteScheduledChore(SelectedChore);
            ScheduledChores = _repo.GetScheduledChores(AllChores, AllUsers, CurrentUser);
            OnPropertyChanged("ScheduledChores");
        }

        /// <summary>
        /// Removes the selected chore from the schedule and then removes it from the directory
        /// </summary>
        public void DeleteChore()
        {
            _repo.DeleteChore(SelectedChore);
            AllChores = _repo.GetChores();
            OnPropertyChanged("AllChores");
            ScheduledChores = _repo.GetScheduledChores(AllChores, AllUsers, CurrentUser);
            OnPropertyChanged("ScheduledChores");
        }

        /// <summary>
        /// Removes the selected chore from the schedule
        /// </summary>
        public void DeleteScheduledChore()
        {
            _repo.DeleteScheduledChore(SelectedChore);
            ScheduledChores = _repo.GetScheduledChores(AllChores, AllUsers, CurrentUser);
            OnPropertyChanged("ScheduledChores");
        }

        /// <summary>
        /// Required for IPropertyChanged
        /// </summary>
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
