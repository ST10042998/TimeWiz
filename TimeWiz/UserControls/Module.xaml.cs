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
using TimeWiz.Classes;
using System.Windows.Markup;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
using TimeWiz.LoginAndRegister;
using System.Runtime.Remoting.Messaging;

namespace TimeWiz.UserControls
{
  
    /// <summary>
    /// Interaction logic for Module.xaml
    /// </summary>
    public partial class Module : UserControl
    {
        //databse objects
        private Semesters semesters= new Semesters();
        private ModuleTables moduleTables = new ModuleTables();
      
        private LoginInfos loginfo = new LoginInfos();

        int semester_id = 0;
        int numWeeks = 0;
        int student_id = 0;
        int login_Id = 0;

        /// <summary>
        ///  creating obj of study class
        /// </summary>
        private StudyClass study;

        /// <summary>
        /// creating obj of calculation class
        /// </summary>
        private CalculationClass cal;

        private StudyClass currentSemester;
        /// <summary>
        /// list that holds module data for my calculations
        /// </summary>
        private List<int> moduleData = new List<int>();

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Constructor
        /// </summary>
        public Module(StudyClass study,CalculationClass cal,Semesters sem)
        {
            InitializeComponent();
           
            // Store the study object
            this.study = study;
            this.cal = cal;
            semesters = sem;
            currentSemester = new StudyClass();
           
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

                sc.StartDate = txtStartDate.Text.Trim();
                sc.EndDate = cal.CalculateEndOfSemester(Convert.ToDateTime(sc.StartDate), sc.NumberOfWeeks);
           

            if (int.TryParse(txtSemester.Text.Trim(), out int semNum))
            {
                sc.SemesterNum = semNum;
                currentSemester.semesterData.semester.SemesterNum = semNum;
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
                currentSemester.semesterData.SemesterList.Add(sc);
                numWeeks= sc.NumberOfWeeks;

                login_Id = loginfo.GetLastAdded().Login_Id;

                var newSemester = semesters.AddSemesterAdo(sc.SemesterNum, sc.NumberOfWeeks, sc.StartDate, sc.EndDate, moduleTables.GetStudentId(login_Id));

                if (newSemester != null)
                {
                    semester_id = newSemester.Semester_Id;
                    this.lblSaved.Content = $"Semester {newSemester.SemesterNum} saved successfully";
                }
            }
            else
            {
                MessageBox.Show("Can't save semester due to invalid data input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            if (isValid)
            {
                currentSemester.semesterData.ModuleList.Add(mc);
                moduleData.Add(mc.Credits);
                moduleData.Add(mc.ClassHoursPerWeek);
                try {
                    // Calculate the SelfStudyHours for the current module
                
                    foreach (ModuleClass moduleData in currentSemester.semesterData.ModuleList)
                    {
                        int numberOfWeeks = numWeeks ;                        
                        int classHoursPerWeek = moduleData.ClassHoursPerWeek;
                        int credit = moduleData.Credits;
                        string code = moduleData.Code;

                        // Calculate the SelfStudyHours for the current module
                        int selfStudyHours = cal.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credits);

                        // Update the SelfStudyHours property of the current module
                        if (selfStudyHours != 0)
                        {
                            mc.SelfStudyHours = selfStudyHours;
                        }
                        else
                        {
                            this.lblErrorS.Content = "Class Hours cant be more than number of weeks";
                            isValid = false;
                        }
                    }
                   
                    // entity is giving error so i used ado.net to save module data
                    var module = moduleTables.AddModuleUsingADO(mc.Name, mc.Code, mc.Credits, semester_id, mc.ClassHoursPerWeek, mc.SelfStudyHours);

                
                    if (module != null)
                    {
                        txtName.Clear();
                        txtCode.Clear();
                        txtCredits.Clear();
                        txtHours.Clear();
                        this.lblSaved.Content = $"Module {mc.Name} saved successfully";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    MessageBox.Show("Module insertion failed.");

                   
                }  
        
            }
            else
            {
                // Error message if isValid returns false
                this.lblSaved.Content = "Cannot save module due to insufficient or invalid input data";
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

               // currentSemester.semesterData.ModuleList.Clear();

                
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
           

             if (!string.IsNullOrEmpty(txtSemester.Text) && !string.IsNullOrEmpty(txtNumWeekSestr.Text) && !string.IsNullOrEmpty(txtStartDate.Text))
            {
                this.lblSaved.Content = string.Empty;
                this.SaveSemesterData();
              

                currentSemester.semesterData.SemesterList.Clear();

                
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// done button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            // Check if there are any modules associated with the current semester
            if (semesters.GetAllSemesterAdo(moduleTables.GetStudentId(login_Id)).Any())
            {
                txtSemester.Clear();
                txtNumWeekSestr.Clear();
                txtStartDate.SelectedDate = null;

                MessageBox.Show("Semester data successfully saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);


                //new instance of module usercontrol to help with dataSaving 
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.NavigateToAddModule();
                }
            }
            else
            {
                MessageBox.Show("Cannot save semester without saving at least one module.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------------------END..Eugene*
