using Housekeeper.Model;
using Housekeeper.ViewModel;
using System.Windows;

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
            _main.Login(selectedUser.ID);
        }
    }
}
