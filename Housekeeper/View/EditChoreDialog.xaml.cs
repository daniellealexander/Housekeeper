using Housekeeper.Model;
using Housekeeper.ViewModel;
using System;
using System.Windows;

namespace Housekeeper.View
{
    public partial class EditChoreDialog : Window
    {
        #region Member Variables

        private MainViewModel _main;

        #endregion Member Variables

        #region Constructors

        public EditChoreDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public bool DeletingChore { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Saves the edits to the chore to the relevent database tables
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryBox.SelectedValue == null ||
                string.IsNullOrEmpty(TaskBox.Text) ||
                FrequencyBox.Text == null ||
                DurationBox.Text == null ||
                PerformBox.Text == null)
            {
                MessageBox.Show("Please fill out all fields before saving this chore.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_main.SelectedChore == null)
                _main.SelectedChore = new ScheduledChore();

            _main.SelectedChore.Category = (Chore.ChoreCategory)Enum.Parse(typeof(Chore.ChoreCategory), CategoryBox.SelectedValue.ToString());
            _main.SelectedChore.Task = TaskBox.Text;
            _main.SelectedChore.Frequency = Convert.ToInt32(FrequencyBox.Text);
            _main.SelectedChore.Duration = Convert.ToInt32(DurationBox.Text);
            _main.SelectedChore.LastPerform = PerformBox.DisplayDate;

            if (_main.EdittingChore)
            {
                User selectedUser = UserBox.SelectedItem as User;
                _main.SelectedChore.UserID = selectedUser.ID;
            }

            DialogResult = true;
            Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this chore from the schedule?", "Delete Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DeletingChore = true;
                DialogResult = true;
                Close();
            }
        }

        private void EditChoreDialog_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _main = DataContext as MainViewModel;
            Title = _main.EdittingChore ? $"Edit Chore - {_main.SelectedChore}" : "Add New Chore to Database";
        }

        #endregion Methods
    }
}
