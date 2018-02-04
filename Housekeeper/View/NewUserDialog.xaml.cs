using Housekeeper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Housekeeper.View
{
    public partial class NewUserDialog : Window
    {
        #region Constructors

        public NewUserDialog(List<User> allusers)
        {
            InitializeComponent();
            UsernameText.Focus();

            AllUsers = allusers;
        }

        #endregion Constructors

        #region Properties

        public List<User> AllUsers { get; set; }
        public string Username { get; set; }

        #endregion Properties

        #region Methods

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameText.Text))
            {
                MessageBox.Show("You must enter a name!", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (AllUsers.Any(u => u.Username.Equals(UsernameText.Text, StringComparison.CurrentCultureIgnoreCase)))
            {
                MessageBox.Show("A user with that name already exists!", "Duplicate Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Username = UsernameText.Text;
            Close();
        }

        #endregion Methods
    }
}
