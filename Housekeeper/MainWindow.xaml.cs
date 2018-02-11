using Housekeeper.Model;
using Housekeeper.View;
using Housekeeper.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Housekeeper
{
    public partial class MainWindow : Window
    {
        #region Member Variables

        private MainViewModel _main;

        #endregion Member Variables

        public MainWindow()
        {
            InitializeComponent();

            _main = new MainViewModel();
            DataContext = _main;
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            _main.Login();
        }

        private void AddUser_OnClick(object sender, RoutedEventArgs e)
        {
            NewUserDialog dlg = new NewUserDialog(_main.AllUsers);
            dlg.ShowDialog();

            if (!string.IsNullOrEmpty(dlg.Username))
            {
                _main.AddUser(dlg.Username);
                UserCombo.SelectedValue = dlg.Username;
            }
        }

        private void AddChore_OnClick(object sender, RoutedEventArgs e)
        {
            EditChoreDialog dlg = new EditChoreDialog(_main.AllUsers);
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                _main.AddChore(dlg.EditableChore);

                // Grab the chore from the collection now that it has been added, to get the ID
                Chore addedChore = _main.AllChores.FirstOrDefault(c =>
                    c.Category.Equals(dlg.EditableChore.Category) && c.Task.Equals(dlg.EditableChore.Task));

                if (dlg.AssignedUser != null)
                    _main.ScheduleChore(addedChore, dlg.AssignedUser);
            }
        }

        private void EditChore_OnClick(object sender, RoutedEventArgs e)
        {
            EditChoreDialog dlg = new EditChoreDialog(_main.SelectedChore, _main.SelectedChore.AssignedTo, _main.AllUsers);
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                _main.EditChore(dlg.EditableChore);
                if (!_main.SelectedChore.AssignedTo.Username.Equals(dlg.AssignedUser.Username))
                    _main.ScheduleChore(dlg.EditableChore, dlg.AssignedUser);
            }
        }

        private void CompleteChore_OnClick(object sender, RoutedEventArgs e)
        {
            _main.CompleteChore();
        }

        private void UserCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _main.UpdateProperties();
        }
    }
}
