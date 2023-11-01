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
using System.Windows.Shapes;

namespace TimeWiz.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.NavigateToLogin();
        }

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
    }
}

