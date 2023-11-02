using MyTimeWizClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TimeWiz.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
      
        /// <summary>
        /// Constructor for the login window
        /// </summary>
        public LoginWindow()
        {
           
            InitializeComponent();
            this.NavigateToLogin();
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///Method that displays the page depending on the menu item user chooses
        /// </summary>
        /// <param name="page"></param>
        private void NavigateToUserControl(UserControl userControl)
        {
            // Unload the current content by setting it to null
            MainInput.Content = null;

            // Navigate to the new userControl
            MainInput.Content = userControl;

        }



        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to go to login display
        /// </summary>
        public void NavigateToLogin()
        {
            NavigateToUserControl(new MyLogin());
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to go to home display
        /// </summary>
        public void NavigateToRegister()
        {
            NavigateToUserControl(new MyRegister());
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to go to home display and using thread to open new window
        /// </summary>
        public void NavigateToApp()
        {

            try
            {
                Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.IsBackground = true;
                newWindowThread.Start();
            }
            catch (Exception ex)
            {
                  
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        private void ThreadStartingPoint()
        {
            try
            {
                MainWindow main = new MainWindow();
                main.Show();
                System.Windows.Threading.Dispatcher.Run();
            }
            catch (Exception ex)
            {
                // logging it  
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to hide the window
        /// </summary>
        public void CloseWindow()
        {
            if (this is Window window)
            {
                window.Hide();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to exit the window
        /// </summary>
        public void exitWindow()
        {
            if (this is Window window)
            {
                window.Close();
            }
        }



    }
}
