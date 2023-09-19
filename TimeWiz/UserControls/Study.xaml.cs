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
    /// Interaction logic for Study.xaml
    /// </summary>
    public partial class Study : UserControl
    {
        //initializing obj
        private CalculationClass cal = new CalculationClass();
        private StudyClass study;
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
      
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="study"></param>
        /// <param name="semester"></param>
        public Study(StudyClass study)
        {
            InitializeComponent();

            // Store the semester object
            this.study = study;

            // Populate the semester combobox
            this.SemesterData();
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

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
            cmBoxSemester.Items.Clear();

            foreach (var sem in sortedSemester)
            {
                // Exclude empty module code
                if (!string.IsNullOrWhiteSpace(sem.semester.SemesterNum.ToString()))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = sem.semester.SemesterNum.ToString();

                    // Set the Tag property with the associated StudyClass object
                    comboBoxItem.Tag = sem; // Ensure 'study' is a valid StudyClass object

                    this.cmBoxSemester.Items.Add(comboBoxItem);
                }
                else
                {
                    MessageBox.Show("Empty","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// save button event for study menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStudySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedComboBoxItem = cmBoxMCode.SelectedItem as ComboBoxItem;
                if (selectedComboBoxItem == null)
                {
                    MessageBox.Show("Invalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                StudyClass Studysemester = selectedComboBoxItem.Tag as StudyClass;
                if (Studysemester == null)
                {
                    MessageBox.Show("Invalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedModule = FindModuleByCode(selectedComboBoxItem.Content.ToString());
                if (selectedModule == null)
                {
                    MessageBox.Show($"Module with Code '{selectedComboBoxItem.Content.ToString()}' not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(txtStudyHrs.Text.Trim(), out int studiedHours))
                {
                    MessageBox.Show("Invalid Studied Hours input", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DateTime currentDate = DateTime.Now.Date;
                if (selectedModule.StudiedHoursPerDate.ContainsKey(currentDate))
                {
                    selectedModule.StudiedHoursPerDate[currentDate] += studiedHours;
                }
                else
                {
                    selectedModule.StudiedHoursPerDate[currentDate] = studiedHours;
                }

                selectedModule.StudiedHours = studiedHours;

                if (this.cal.CalculateRemainingHoursForCurrentWeek(selectedModule) > 0)
                {
                    selectedModule.RemainingWeekHours = this.cal.CalculateRemainingHoursForCurrentWeek(selectedModule);
                    selectedModule.Progressbar = cal.ProgressBarCal(selectedModule);
                }
                else
                {
                    MessageBox.Show("All self-study hours for the week have been completed", "Completed weekly self-study hours", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show($" {selectedModule.Progressbar}");

                txtStudyHrs.Clear();
                MessageBox.Show($"Studied Hours for {selectedModule.Name} on {currentDate.ToShortDateString()} saved: {studiedHours}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// Finding module by using modulecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private ModuleClass FindModuleByCode(string code)
        {
            var foundModule = study.SemesterList
                .SelectMany(semester => semester.ModuleList)
                .FirstOrDefault(module => module.Code == code);

            if (foundModule != null)
            {
                // Module with the specified code found, return it
                return foundModule;
            }

            // Module with the specified code not found
            return null;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// handles the ComboBox's selection change event, updating data grids and refreshing a combo box based on the selected semester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmBoxSemester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Retrieve the selected item (ComboBoxItem) from the ComboBox
            ComboBoxItem selectedComboBoxItem = cmBoxSemester.SelectedItem as ComboBoxItem;

            if (selectedComboBoxItem != null)
            {
                // Retrieve the semester object from the ComboBox item's Tag
                SemesterDataClass Studysemester = selectedComboBoxItem.Tag as SemesterDataClass;

                if (Studysemester != null)
                {
                    // Display the selected semester in a MessageBox or any other control you prefer

                    semesterDataGrid.ItemsSource = Studysemester.SemesterList;
                    semesterDataGrid.Items.Refresh();

                    moduleDataGrid.ItemsSource = Studysemester.ModuleList;
                    moduleDataGrid.Items.Refresh();

                    // Update the items in cmBoxMCode based on the modules of the selected semester
                    RefreshComboBox(Studysemester);
                 
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


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// refreshing the combo box to show the Module Code
        /// </summary>
        private void RefreshComboBox(SemesterDataClass modules)
        {
            // Sort the Module code in alphabetical order
            var sortModuleCode = modules.ModuleList.OrderBy(m => m.Code).ToList();

            // Clear the current items
            this.cmBoxMCode.Items.Clear();

            // Add the sorted module codes to the combo box
            foreach (var module in sortModuleCode)
            {
                // Exclude empty module code
                if (!string.IsNullOrWhiteSpace(module.Code))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = module.Code;
                    comboBoxItem.Tag = study;
                    this.cmBoxMCode.Items.Add(comboBoxItem);
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Cancel button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStudyCancel_Click(object sender, RoutedEventArgs e)
        {
            var option = MessageBox.Show("Cancel the operation?", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (option == MessageBoxResult.Yes)
            {
                // Call the NavigateToHome method of the MainWindow
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToHome();
                }
            }
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End...
