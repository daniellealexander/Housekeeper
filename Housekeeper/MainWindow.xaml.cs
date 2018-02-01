using Housekeeper.Model;
using Housekeeper.View;
using Housekeeper.ViewModel;
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
            User selectedUser = UserCombo.SelectedItem as User;
            _main.Login(selectedUser.Username);
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
    }
}
