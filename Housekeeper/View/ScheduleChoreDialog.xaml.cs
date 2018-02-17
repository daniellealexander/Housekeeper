using Housekeeper.Model;
using Housekeeper.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Housekeeper.View
{
    public partial class ScheduleChoreDialog : Window
    {
        #region Member Variables

        private MainViewModel _main;

        #endregion Member Variables

        #region Constructors

        public ScheduleChoreDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Limits the Task box to the chores for the selected category
        /// </summary>
        private void CategoryBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCategory = Convert.ToString(CategoryBox.SelectedItem);
            _main.CategorizedChores = new List<Chore>(_main.AllChores.Where(c => c.CategoryString.Equals(selectedCategory)));
            _main.OnPropertyChanged("CategorizedChores");

            ChoreBox.IsEnabled = CategoryBox.SelectedItem != null;
        }

        /// <summary>
        /// Creates a scheduled chore and saves it to the relevant database table
        /// </summary>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryBox.SelectedItem == null ||
                ChoreBox.SelectedItem == null ||
                UserBox.SelectedItem == null)
            {
                MessageBox.Show("Please fill out all fields before scheduling this chore.", "Incomplete Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            _main.SelectedChore = new ScheduledChore();
            _main.SelectedChore.Category = (Chore.ChoreCategory)Enum.Parse(typeof(Chore.ChoreCategory), CategoryBox.SelectedValue.ToString());

            Chore selectedChore = ChoreBox.SelectedItem as Chore;
            _main.SelectedChore.Task = selectedChore.Task;
            _main.SelectedChore.ChoreID = selectedChore.ID;

            User selectedUser = UserBox.SelectedItem as User;
            _main.SelectedChore.UserID = selectedUser.ID;

            DialogResult = true;
            Close();
        }

        private void ScheduleChoreDialog_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _main = DataContext as MainViewModel;
            _main.CategorizedChores = new List<Chore>();
            _main.OnPropertyChanged("CategorizedChores");
        }

        #endregion Methods
    }
}
