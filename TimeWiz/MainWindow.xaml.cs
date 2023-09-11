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

namespace TimeWiz
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StudyClass study;
        private SemesterClass semester;
        public MainWindow()
        {
            InitializeComponent();
            study = new StudyClass();
            semester = new SemesterClass();
            // Navigates to the home page when the application starts
            NavigateToUserControl(new UserControls.Home());
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// frame changes to appropraite display if user chooses/clicks on any of the menu buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (sender is Button button)
                {
                    switch (button.Name)
                    {
                        case "HomeBtn":
                            NavigateToUserControl(new UserControls.Home());
                            break;
                        case "ModuleBtn":
                            NavigateToUserControl(new UserControls.Module(study,semester));
                            break;
                        case "StudyBtn":
                            NavigateToUserControl(new UserControls.Study(study,semester));
                            break;
                        case "ViewBtn":
                            NavigateToUserControl(new UserControls.View());
                            break;
                    }
                }
            } 
            catch (Exception ex)
            {
                // Handle the exception here,and shows an error message.
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// c
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///Method that displays the page depending on the menu item user chooses
        /// </summary>
        /// <param name="page"></param>
        public void NavigateToUserControl(UserControl userControl)
        {
            // Unload the current content by setting it to null
            MainFrame.Content = null;

            // Navigate to the new userControl
            MainFrame.Content = userControl;

        }

    }
}
//-----------------------------------------------------------------------------------------------------------------------------------------------------END..Eugene*