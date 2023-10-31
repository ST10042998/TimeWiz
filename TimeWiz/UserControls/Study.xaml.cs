﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
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
using TimeWiz.Classes;

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

        private Semesters semester = new Semesters();
        private ModuleTables module;
        private StudyTables studyTable ;
        private SqlConnection connection;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// dictionary property to store the recorded hours for this module
        /// </summary>
        public Dictionary<DateTime, int> StudiedHoursPerDate { get; } = new Dictionary<DateTime, int>();

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
            //semester = new Semesters();
            module = new ModuleTables();
            studyTable = new StudyTables();
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// populating my semester combobox
        /// </summary>
        private void SemesterData()
        {
           ;
            // Sort the semesters in alphabetical order
            var sortedSemester = semester.GetAllSemesterAdo()
                .OrderBy(s => s.SemesterNum)
                .ToList();

            // Clear the current items
            cmBoxSemester.Items.Clear();

            foreach (var sem in sortedSemester)
            {
                // Exclude empty module code
                if (!string.IsNullOrWhiteSpace(sem.SemesterNum.ToString()))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = sem.SemesterNum.ToString();

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
        /// save button event to capture the study hous 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStudySave_Click(object sender, RoutedEventArgs e)
        {
            ModuleClass mc = new ModuleClass();
            try
            {
                ComboBoxItem selectedComboBoxItem = cmBoxMCode.SelectedItem as ComboBoxItem;

                MessageBox.Show($"Selected Module: {selectedComboBoxItem.Content}");
                if (selectedComboBoxItem == null)
                {
                    MessageBox.Show("Invalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               /* Semester semester = selectedComboBoxItem.Tag as Semester;
                if (semester == null)
                {
                    MessageBox.Show("hereInvalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }*/

                var selectedModule = FindModuleByCode(selectedComboBoxItem.Content.ToString());

                MessageBox.Show($"Selected Module: {selectedModule.Name}");
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
                try
                {
                    DateTime currentDate = DateTime.Now.Date;
                    var hours = 0;
                    hours += studiedHours;

                    // theres only 24 hours in a day so you cant add more hours than that 
                    if (hours <= 24)
                    {
                        // Assuming you have the selected module's Module_Id
                        int selectedModuleId = selectedModule.Module_Id;

                        // Access the study associated with the same module ID
                        var selected = studyTable.GetStudyByID(selectedModuleId).FirstOrDefault();

                     
                        if (selected != null)
                        {
                            mc.StudiedHours += studiedHours;
                            MessageBox.Show($"Studied {studiedHours} hours for {selectedModule.Name} on {currentDate.ToShortDateString()}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                            if (mc.StudiedHours <= selectedModule.SelfStudyHours)

                            {
                                StudiedHoursPerDate.Add(currentDate, studiedHours);
                                // Calculate ProgressBarPercentage based on your business logic
                                mc.Progressbar = cal.ProgressBarCal(StudiedHoursPerDate, selectedModule.SelfStudyHours.Value);
                                mc.RemainingWeekHours = this.cal.CalculateRemainingHoursForCurrentWeek(StudiedHoursPerDate, selectedModule.SelfStudyHours.Value);

                                // Save changes to the database
                                studyTable.UpdateStudy(selectedModuleId, mc.StudiedHours, mc.RemainingWeekHours, selectedModuleId, Convert.ToDecimal(mc.Progressbar), DateTime.Now);

                                txtStudyHrs.Clear();
                                MessageBox.Show($"Studied {mc.StudiedHours} hours for {selectedModule.Name} on {currentDate.ToShortDateString()}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                mc.StudiedHours -= studiedHours;
                                MessageBox.Show("The amount of self-study hours is more than assigned.", "Self-study hours", MessageBoxButton.OK, MessageBoxImage.Information);
                                txtStudyHrs.Clear();
                            }
                        }
                        else if(selected == null)
                        {
                            mc.StudiedHours = 0;
                            mc.StudiedHours += studiedHours;
                            MessageBox.Show($"Studied {studiedHours} hours for {selectedModule.Name} on {currentDate.ToShortDateString()}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                            if (mc.StudiedHours <= selectedModule.SelfStudyHours)

                            {
                            

                                StudiedHoursPerDate.Add(currentDate, studiedHours);
                                // Calculate ProgressBarPercentage based on your business logic
                                mc.Progressbar = cal.ProgressBarCal(StudiedHoursPerDate, selectedModule.SelfStudyHours.Value);
                                mc.RemainingWeekHours = this.cal.CalculateRemainingHoursForCurrentWeek(StudiedHoursPerDate, selectedModule.SelfStudyHours.Value);

                                // Save changes to the database
                                studyTable.AddStudy( mc.StudiedHours, mc.RemainingWeekHours, selectedModuleId, Convert.ToDecimal(mc.Progressbar), DateTime.Now);

                                txtStudyHrs.Clear();
                                MessageBox.Show($"Studied {selected.StudiedHours} hours for {selectedModule.Name} on {currentDate.ToShortDateString()}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                mc.StudiedHours -= studiedHours;
                                MessageBox.Show("The amount of self-study hours is more than assigned.", "Self-study hours", MessageBoxButton.OK, MessageBoxImage.Information);
                                txtStudyHrs.Clear();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No study record found for this module", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        hours -= studiedHours;
                        MessageBox.Show("Studied hours entered is more hours thats in one day(24 hours)", "Weekly self-study hours", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtStudyHrs.Clear();
                    }
                }catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Finding module by using modulecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ModuleTable FindModuleByCode(string code)
        {
            using (var db = new MyTimeWizDatabaseEntities1())
            {
                var module = db.ModuleTables.Where(m => m.Code == code).FirstOrDefault();
                return module;
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// handles the ComboBox's selection change event, updating data grids and refreshing a combo box based on the selected semester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmBoxSemester_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = cmBoxSemester.SelectedItem as ComboBoxItem;
            try
            {
                if (selectedComboBoxItem != null)
                {
                    Semester selectedSemester = selectedComboBoxItem.Tag as Semester;
                    MessageBox.Show($"Selected Semester: {selectedSemester.SemesterNum}");

                    semesterDataGrid.ItemsSource = new List<Semester> { selectedSemester };


                    var semesterModules = module.GetAllModules(selectedSemester);
                    moduleDataGrid.ItemsSource = semesterModules;



                    RefreshComboBox(selectedSemester);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// refreshing the combo box to show the Module Code
        /// </summary>
        private void RefreshComboBox(Semester modules)
        {
            // Sort the Module code in alphabetical order
            var sortModuleCode = module.GetAllModules(modules).OrderBy(m => m.Code).ToList();

            // Clear the current items
            this.cmBoxMCode.Items.Clear();

               
            // Add the sorted module codes to the combo box
            foreach (var myModule in sortModuleCode)
            {
                
                if (!string.IsNullOrWhiteSpace(myModule.Code))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = myModule.Code;
                    comboBoxItem.Tag = module; 
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