using Housekeeper.Model;
using System;
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

        public bool ShowLogin { get; set; }
        public bool AllowLogin { get { return CurrentUser != null; } }
        public bool LoggedIn { get { return !ShowLogin; } }

        #endregion Properties

        #region Methods

        private void InitializeCollections()
        {
            AllUsers = _repo.GetUsers();
            AllChores = _repo.GetChores();
            ScheduledChores = _repo.GetScheduledChores();

            InterpolateSchedule();
        }

        private void InterpolateSchedule()
        {
            foreach (ScheduledChore chore in ScheduledChores)
            {
                chore.Chore = AllChores.FirstOrDefault(c => c.ID == chore.ChoreID);
                chore.AssignedTo = AllUsers.FirstOrDefault(u => u.ID == chore.UserID);
            }

            OnPropertyChanged("ScheduledChores");
        }

        public void Login(string userName)
        {
            CurrentUser = AllUsers.FirstOrDefault(u => u.Username.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            if (CurrentUser == null) return;

            ShowLogin = false;
            OnPropertyChanged("ShowLogin");
            OnPropertyChanged("LoggedIn");
        }

        public void AddUser(string userName)
        {
            _repo.AddUser(userName);
            AllUsers = _repo.GetUsers();
            OnPropertyChanged("AllUsers");
        }

        public void UpdateProperties()
        {
            OnPropertyChanged("AllowLogin");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
