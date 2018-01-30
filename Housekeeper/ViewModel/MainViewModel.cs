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

        public bool ShowLogin { get; set; }

        #endregion Properties

        #region Methods

        private void InitializeCollections()
        {
            AllUsers = _repo.GetUsers();
        }

        public void Login(int userIndex)
        {
            CurrentUser = AllUsers.FirstOrDefault(u => u.ID == userIndex);
            ShowLogin = false;
            OnPropertyChanged("ShowLogin");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}
