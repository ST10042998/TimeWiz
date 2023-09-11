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
        /// Instance of main window class
        /// </summary>
        private MainWindow main = new MainWindow();
        private SemesterClass semester;
        private StudyClass study;
        private ModuleClass module;
        //----------------------------------------------------------------------------------------------------------------------------------------------------------
        List<int> moduleData = new List<int>();
        /// <summary>
        /// Constructor
        /// </summary>
        public Module(StudyClass study,SemesterClass semester)
        {
            InitializeComponent();
            module = new ModuleClass();

            // Store the study object
            this.study = study;
            this.semester = semester;

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that saves module data 
        /// </summary>
        /// <param name="mc"></param>
        public void SaveModuleData()
        {
            //creating a new obj to save module data 
            ModuleClass mc = new ModuleClass();

            mc.Name = txtName.Text.Trim();
            mc.Code = txtCode.Text.Trim();

            if(int.TryParse(txtCredits.Text.Trim(), out int credits))
            {
                mc.Credits = credits;
            }
            else
            {
                // Display an error message or handle the invalid input appropriately
                MessageBox.Show("Invalid Module Credits input" , "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (int.TryParse(txtHours.Text.Trim(), out int hours))
            {
                mc.ClassHoursPerWeek = hours;
            }
            else
            {
                // Display an error message or handle the invalid input appropriately
                MessageBox.Show("Invalid Module ClassHoursPerWeek input" , "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            semester.ModuleList.Add(mc);
            moduleData.Add(mc.Credits);
            moduleData.Add(mc.ClassHoursPerWeek);

            txtName.Clear();
            txtCode.Clear();
            txtCredits.Clear();
            txtHours.Clear();

        }

      



        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that saves semester data
        /// </summary>
        /// <param name="sc"></param>
        public void SaveSemesterData()
        {
            //Creating new obj to save semester data
            SemesterClass sc = new SemesterClass();
            

            if (int.TryParse(txtNumWeekSestr.Text.Trim(), out int Weeks))
            {
                sc.NumberOfWeeks = Weeks;
            }
            else
            {
                // Display an error message or handle the invalid input appropriately
                MessageBox.Show("Invalid Number of weeks input" , "Invalid",MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            sc.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            study.Semester.Add(sc);

            foreach (ModuleClass moduleData in semester.ModuleList)
            {
                int numberOfWeeks = sc.NumberOfWeeks;
                int classHoursPerWeek = moduleData.ClassHoursPerWeek;
                int credits = moduleData.Credits;
                string code = moduleData.Code;


                // Find the module with the matching moduleCode in your semester.ModuleList
                var module1 = semester.ModuleList.FirstOrDefault(m => moduleData.Code == code);
                if (module1 != null)
                {
                    // Set the SelfStudyHours property of the module
                    module1.SelfStudyHours = study.CalculateSelfStudyHours(code, numberOfWeeks, classHoursPerWeek, credits);
                }
                else
                {
                    MessageBox.Show($"Module with code {code} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }

            
            txtNumWeekSestr.Clear();
            txtStartDate.SelectedDate = null;

           
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Save Module Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveModule_Click(object sender, RoutedEventArgs e)
        {
            
            this.SaveModuleData();
            MessageBox.Show($"Module {module.Name} successfully saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);


        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Save Semester event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSemester_Click(object sender, RoutedEventArgs e)
        {

            this.SaveSemesterData();
            MessageBox.Show("Data successfully saved", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            
              
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Cancel button event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelSave_Click(object sender, RoutedEventArgs e)
        {
           var option = MessageBox.Show("Cancelling ..." , "Cancel", MessageBoxButton.YesNo , MessageBoxImage.Warning);
            if (option == MessageBoxResult.Yes)
            {
                main.NavigateToUserControl(new UserControls.Home());
            }
           
        }
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------------------END..Eugene*
