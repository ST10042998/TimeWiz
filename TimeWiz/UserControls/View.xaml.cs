using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyTimeWizClassLib;

namespace TimeWiz.UserControls
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        //creating obj for StudyClass
        private StudyClass study;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
     
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="study"></param>
        public View(StudyClass study)
        {
            InitializeComponent();

            // Store the semester object
            this.study = study;

            // Populate the semester combobox
            this.SemesterData();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// populating my semester combobox
        /// </summary>
        private void SemesterData()
        {
            // Sort the semesters in alphabetical order
            var sortedSemester = study.SemesterList
                .OrderBy(s => s.semester.SemesterNum)
                .ToList();

            // Clear the current items
            cmBoxSemest.Items.Clear();

            foreach (var sem in sortedSemester)
            {
                // Exclude empty module code
                if (!string.IsNullOrWhiteSpace(sem.semester.SemesterNum.ToString()))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = sem.semester.SemesterNum.ToString();

                    // Set the Tag property with the associated StudyClass object
                    comboBoxItem.Tag = sem; // Ensure 'study' is a valid StudyClass object

                    this.cmBoxSemest.Items.Add(comboBoxItem);
                }
                else
                {
                    MessageBox.Show("Empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// handles the ComboBox's selection change event, updating data grids and refreshing a combo box based on the selected semester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmBoxSemest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Retrieve the selected item (ComboBoxItem) from the ComboBox
            ComboBoxItem selectedComboBoxItem = cmBoxSemest.SelectedItem as ComboBoxItem;

            if (selectedComboBoxItem != null)
            {
                // Retrieve the semester object from the ComboBox item's Tag
                SemesterDataClass Studysemester = selectedComboBoxItem.Tag as SemesterDataClass;

                if (Studysemester != null)
                {
                    // Display the selected semester in a MessageBox or any other control you prefer

                    DatagrdViewSem.ItemsSource = Studysemester.SemesterList;
                    DatagrdViewSem.Items.Refresh();

                    DatagrdViewModule.ItemsSource = Studysemester.ModuleList;
                    DatagrdViewModule.Items.Refresh();

                }
                else
                {
                    // Handle the case where selectedStudy is null, e.g., display an error message.
                    MessageBox.Show("Invalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("No item selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// deleting a semester (button click event)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            // Retrieve the selected item (ComboBoxItem) from the ComboBox
            ComboBoxItem selectedComboBoxItem = cmBoxSemest.SelectedItem as ComboBoxItem;

            if (selectedComboBoxItem != null)
            {
                // Retrieve the semester object from the ComboBox item's Tag
                SemesterDataClass Studysemester = selectedComboBoxItem.Tag as SemesterDataClass;

                if (Studysemester != null)
                {
                    var option = MessageBox.Show($"Are you sure you want to delete semester {Studysemester.semester.SemesterNum}","Delete",MessageBoxButton.YesNo,MessageBoxImage.Warning);
                   if( option == MessageBoxResult.Yes)
                   {
                        // Remove the Studysemester from the SemesterList
                        study.SemesterList.Remove(Studysemester);
 
                        Studysemester.ModuleList.Clear();
                        Studysemester.SemesterList.Clear();

                        //refreshing my datagrids after deleting semester
                        DatagrdViewSem.Items.Refresh();
                        DatagrdViewModule.Items.Refresh();
                        
                        MessageBox.Show($"Successfully deleted semester", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Remove the selected ComboBoxItem from the ComboBox
                        cmBoxSemest.Items.Remove(selectedComboBoxItem);
                        cmBoxSemest.SelectedItem = null;
                   }
                    else
                    {
                       MessageBox.Show($"Deletion of semester{Studysemester.semester.SemesterNum} cancelled ", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..