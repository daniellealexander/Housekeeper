using System.Collections.Generic;
using System.Windows;
using Housekeeper.Model;

namespace Housekeeper.View
{
    public partial class EditChoreDialog : Window
    {
        #region Constructors

        public EditChoreDialog(List<User> allUsers)
        {
            InitializeComponent();

            IsAddingChore = true;
            Title = "Add New Chore";
            AllUsers = allUsers;
        }

        public EditChoreDialog(Chore chore, User assignedUser, List<User> allUsers)
        {
            InitializeComponent();

            Title = "Edit Chore";
            EditableChore = chore;
            AssignedUser = assignedUser;
            AllUsers = allUsers;
        }

        #endregion Constructors

        #region Properties

        public List<User> AllUsers { get; set; }
        public Chore EditableChore { get; set; }
        public User AssignedUser { get; set; }

        public bool IsAddingChore { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Saves the edits to the chore to the relevent database tables
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (EditableChore.Category == null ||
                string.IsNullOrEmpty(EditableChore.Task) ||
                EditableChore.Frequency == null ||
                EditableChore.Duration == null ||
                EditableChore.LastPerform == null)
            {
                MessageBox.Show("Please fill out all fields before saving this chore.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        #endregion Methods
    }
}
