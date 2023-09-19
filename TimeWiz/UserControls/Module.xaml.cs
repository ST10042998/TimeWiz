using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    /// Interaction logic for Module.xaml
    /// </summary>
    public partial class Module : UserControl
    {
      
        /// <summary>
        ///  creating obj of study class
        /// </summary>
        private StudyClass study;

        /// <summary>
        /// creating obj of calculation class
        /// </summary>
        private CalculationClass cal;
        private SemesterDataClass currentSemester ;

        /// <summary>
        /// list that holds module data for my calculations
        /// </summary>
        private List<int> moduleData = new List<int>();

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        public Module(StudyClass study,CalculationClass cal)
        {
            InitializeComponent();
           
            // Store the study object
            this.study = study;
            this.cal = cal;
            currentSemester = new SemesterDataClass();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that saves module data 
        /// </summary>
        /// <param name="mc"></param>
        public void SaveModuleData()
        {
            // Initialize a flag to check if all input is valid
            bool isValid = true;

            // Creating a new object to save module data
            ModuleClass mc = new ModuleClass();

            //when saving i make the first lette rof the module name uppercase
            mc.Name = char.ToUpper(txtName.Text[0]) + txtName.Text.Substring(1);

            //when saving make the module code uppercase 
            mc.Code = txtCode.Text.Trim().ToUpper();

            // Validate and parse Credits input
            if (int.TryParse(txtCredits.Text.Trim(), out int credits))
            {
                mc.Credits = credits;
            }
            else
            {
                // Display an error message to handle the invalid input appropriately
                this.lblErrorM.Content = "Invalid Module Credits input";
                this.txtCredits.Clear();
                isValid = false;
            }

            // Validate and parse ClassHoursPerWeek input
            if (int.TryParse(txtHours.Text.Trim(), out int hours))
            {
                mc.ClassHoursPerWeek = hours;
            }
            else
            {
                // Display an error message to handle the invalid input appropriately
                this.lblErrorM.Content = "Invalid Module ClassHoursPerWeek input";
                this.txtHours.Clear();
                isValid = false;
            }

            // Save module data only if all input is valid
            if (isValid)
            {
                // Add the module to the list of modules for the current semester
                currentSemester.ModuleList.Add(mc);
                moduleData.Add(mc.Credits);
                moduleData.Add(mc.ClassHoursPerWeek);

                txtName.Clear();
                txtCode.Clear();
                txtCredits.Clear();
                txtHours.Clear();
                this.lblSaved.Content = $"Module {mc.Name} saved successfully";
            }
            else
            { 
                //error message if isvalid returns false
                this.lblSaved.Content = "Cannot save module due to insufficient or invalid input data";
                return;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that saves semester data
        /// </summary>
        /// <param name="sc"></param>
        public void SaveSemesterData()
        {
            // Initialize a flag to check if all input is valid
            bool isValid = true;

            SemesterClass sc = new SemesterClass();

            if (int.TryParse(txtNumWeekSestr.Text.Trim(), out int Weeks))
            {
                sc.NumberOfWeeks = Weeks;
            }
            else
            {
                // Display an error message or handle the invalid input appropriately
                this.lblErrorS.Content = "Invalid Number of weeks input";
                isValid = false;
            }

            sc.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            

            if (int.TryParse(txtSemester.Text.Trim(), out int semNum))
            {
                sc.SemesterNum = semNum;
                currentSemester.semester.SemesterNum = semNum;
            }
            else
            {
                // Display an error message or handle the invalid input appropriately
                this.lblErrorS.Content = "Invalid Semester number input";
                isValid = false;
            }
            // Save module data only if all input is valid
            if (isValid)
            {
                currentSemester.SemesterList.Add(sc);

                study.SemesterList.Add(currentSemester);

                foreach (ModuleClass moduleData in currentSemester.ModuleList)
                {
                    int numberOfWeeks = sc.NumberOfWeeks;
                    int classHoursPerWeek = moduleData.ClassHoursPerWeek;
                    int credits = moduleData.Credits;
                    string code = moduleData.Code;

                    // Calculate the SelfStudyHours for the current module
                    int selfStudyHours = cal.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credits);

                    // Update the SelfStudyHours property of the current module
                    moduleData.SelfStudyHours = selfStudyHours;
                }
                txtSemester.Clear();
                txtNumWeekSestr.Clear();
                txtStartDate.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Can't Save semester due to insufficient input data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }



        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Save Module Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveModule_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text) &&
                !string.IsNullOrEmpty(txtCredits.Text) &&
                !string.IsNullOrEmpty(txtHours.Text) &&
                !string.IsNullOrEmpty(txtName.Text))
            {
                this.lblErrorM.Content = string.Empty;
                this.SaveModuleData();
            }
            else
            {
                this.lblSaved.Content = "Cannot save module due to insufficient input data";
               
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Save Semester event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSemester_Click(object sender, RoutedEventArgs e)
        {
            // Check if there are any saved modules in the current semester
            if (currentSemester.ModuleList.Count == 0)
            {
                MessageBox.Show("Cannot save semester without saving at least one module.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (!string.IsNullOrEmpty(txtSemester.Text) && !string.IsNullOrEmpty(txtNumWeekSestr.Text) && !string.IsNullOrEmpty(txtStartDate.Text))
            {
                this.lblSaved.Content = string.Empty;
                this.SaveSemesterData();
                MessageBox.Show("Semester data successfully saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
              
                //new instance of module usercontrol to help with dataSaving 
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToAddModule();
                }
            }
            else
            {
                MessageBox.Show("Can't Save semester due to insufficient input data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

            //----------------------------------------------------------------------------------------------------------------------------------------------------------

            /// <summary>
            /// Cancel button event 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void btnCancelSave_Click(object sender, RoutedEventArgs e)
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
//-------------------------------------------------------------------------------------------------------------------------------------------------------END..Eugene*
