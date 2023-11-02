using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using TimeWiz.Classes;
using TimeWiz.LoginAndRegister;

namespace TimeWiz.UserControls
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : UserControl
    {
       //initializing obj
        private CalculationClass cal = new CalculationClass();
        private StudyClass study;

        private Semesters semester = new Semesters();
        private ModuleTables module  = new ModuleTables();
        private LoginInfos loginfo = new LoginInfos();

        int login_Id = 0;

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
            login_Id = loginfo.GetLastAdded().Login_Id;


            // Sort the semesters in alphabetical order
            var sortedSemester = semester.GetAllSemesterAdo(module.GetStudentId(login_Id))
                .OrderBy(s => s.SemesterNum)
                .ToList();


            // Clear the current items
            cmBoxSemest.Items.Clear();

            foreach (var sem in sortedSemester)
            {
                // Exclude empty module code
                if (!string.IsNullOrWhiteSpace(sem.SemesterNum.ToString()))
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = sem.SemesterNum.ToString();

                    // Set the Tag property with the associated StudyClass object
                    comboBoxItem.Tag = sem; // Ensure 'study' is a valid StudyClass object

                    this.cmBoxSemest.Items.Add(comboBoxItem);
                }
                else
                {
                    MessageBox.Show("Empty","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }

        }
        private Semester FindSemesterByNum(string num)
        {
          
            using (var db = new MyTimeWizDatabaseEntity())
            {
                var semester = db.Semesters.Where(s => s.SemesterNum.ToString() == num).First();
                return semester;
            }
        }
      
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// handles the ComboBox's selection change event, updating data grids and refreshing a combo box based on the selected semester
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CmBoxSemest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = cmBoxSemest.SelectedItem as ComboBoxItem;
           
                if (selectedComboBoxItem != null)
                {
                    Semester selectedSemester = selectedComboBoxItem.Tag as Semester;

                var semester = await Task.Run(() => selectedSemester);
                // Retrieve data asynchronously
                // Update the data grids on the main UI thread
                Dispatcher.Invoke(() =>
                {
                    DatagrdViewSem.ItemsSource = new List<Semester> { selectedSemester };
                });

                var semesterModules = await Task.Run(() => module.GetAllModules(selectedSemester));
                // Update the data grids on the main UI thread
                Dispatcher.Invoke(() => 
                {
                     DatagrdViewModule.ItemsSource = semesterModules;
                     
                });



                var selectedSemest = FindSemesterByNum(selectedComboBoxItem.Content.ToString());
                    this.lblWeek.Content = $"Week : {cal.GetCurrentWeek(Convert.ToDateTime(selectedSemester.StartDate), selectedSemest.NumOfWeeks)}";           
                
                }
                else
                {
                    // Handle the case where selectedStudy is null, e.g., display an error message.
                    MessageBox.Show("Invalid selection. Please choose a valid semester.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                Semester Studysemester = selectedComboBoxItem.Tag as Semester;

                if (Studysemester != null)
                {
                    var option = MessageBox.Show($"Are you sure you want to delete semester {Studysemester.SemesterNum}","Delete",MessageBoxButton.YesNo,MessageBoxImage.Warning);
                   if( option == MessageBoxResult.Yes)
                   {
                        //deleting data from database
                       
                        module.DeleteModuleBySemesterId(Studysemester.Semester_Id);

                        semester.DeleteSemester(Studysemester.Semester_Id);


                        this.lblWeek.Content =string.Empty;

                        //refreshing my datagrids after deleting semester
                        Dispatcher.Invoke(() =>
                        {
                            DatagrdViewSem.ItemsSource = null;
                            DatagrdViewSem.Items.Refresh();
                            DatagrdViewModule.ItemsSource = null;
                            DatagrdViewModule.Items.Refresh();
                        });
                        
                        MessageBox.Show($"Successfully deleted semester", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Remove the selected ComboBoxItem from the ComboBox
                        cmBoxSemest.Items.Remove(selectedComboBoxItem);
                        cmBoxSemest.SelectedItem = null;
                   }
                    else
                    {
                       MessageBox.Show($"Deletion of semester{Studysemester.SemesterNum} cancelled ", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..