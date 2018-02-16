using Housekeeper.View;
using Housekeeper.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Housekeeper.Model;

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

        #region Login

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

        private void UserCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _main.UpdateProperties();
        }

        #endregion Login

        #region Schedule

        private void ScheduleChore_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void EditChore_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _main.EdittingChore = true;
                EditChoreDialog dlg = new EditChoreDialog { DataContext = _main };
                dlg.ShowDialog();

                if (dlg.DialogResult == true)
                {
                    if (dlg.DeletingChore)
                    {
                        _main.DeleteScheduledChore();
                    }
                    else
                    {
                        _main.EditChore();
                        _main.ScheduleChore();
                    }
                }
            }
            finally
            {
                _main.EdittingChore = false;
            }
        }

        private void CompleteChore_OnClick(object sender, RoutedEventArgs e)
        {
            _main.CompleteChore();
        }

        #endregion Schedule

        #region Chore Collection

        private void AddChore_OnClick(object sender, RoutedEventArgs e)
        {
            ScheduledChore temp = _main.SelectedChore;

            try
            {
                _main.AddingChore = true;
                _main.SelectedChore = null;

                EditChoreDialog dlg = new EditChoreDialog { DataContext = _main };
                dlg.ShowDialog();

                if (dlg.DialogResult == true)
                    _main.AddChore();
            }
            finally
            {
                _main.AddingChore = false;
                _main.SelectedChore = temp;
            }
        }

        private void DeleteChore_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {_main.SelectedChore} from the directory?", "Delete Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                _main.DeleteScheduledChore();
            }
        }

        #endregion Chore Collection

        private void Schedule_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _main.UpdateProperties();
        }
    }
}
