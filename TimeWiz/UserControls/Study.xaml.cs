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
        private StudyClass study;
        private SemesterClass semester;
        private ModuleClass module;
        public Study(StudyClass study, SemesterClass semester)
        {
            InitializeComponent();
            // Store the study object
            this.study = study;

            // Store the semester object
            this.semester = semester;

            module = new ModuleClass();
            FillDataGrid();
        }

        /// <summary>
        /// method that displays my module/semester data on my datagrid
        /// </summary>
        public void FillDataGrid()

        {
                if (semester.ModuleList == null)
                {
                    MessageBox.Show("NO SAVED DATA", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                moduleDataGrid.ItemsSource = semester.ModuleList;
                moduleDataGrid.Items.Refresh();
            
        }
        
        string moduleCode = string.Empty;
        private void moduleDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Check if the user edited the "Input Study Hours" column and confirmed the edit.
            if (e.Column.Header.ToString() == "Input Study Hours" && e.EditAction == DataGridEditAction.Commit)
            {
                // Get the row index where the user added input study hours.
                int rowIndex = e.Row.GetIndex();

                // Access the corresponding ModuleClass data item 
                ModuleClass editedModule = moduleDataGrid.Items[rowIndex] as ModuleClass;

                // Retrieve the Module Code from the editedModule object.
                moduleCode = editedModule?.Code;

            }
        }


        private void btnStudySave_Click(object sender, RoutedEventArgs e)
        {
            foreach (ModuleClass moduleData in semester.ModuleList)
            {
                // Find the module with the matching moduleCode in your semester.ModuleList
                var module1 = semester.ModuleList.FirstOrDefault(m => module.Code == moduleCode);
                if (module1 != null)
                {
                    // Set the SelfStudyHours property of the module
                    module1.StudiedHours = 
                }
            }
        }
    } 
}
