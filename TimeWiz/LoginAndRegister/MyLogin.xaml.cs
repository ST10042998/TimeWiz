using MyTimeWizClassLib;
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
using TimeWiz.Classes;

namespace TimeWiz.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for MyLogin.xaml
    /// </summary>
    public partial class MyLogin : UserControl
    {
        //create an instance classes
        MyLoginWorker loginWorker = new MyLoginWorker();
        Logins logins = new Logins();
        LoginInfos loginInfos = new LoginInfos();
      
        bool isLogin = false;

        public int Login_Id { get; set; }

        //----------------------------------------------------------------------------------------------------------------------------------

        public MyLogin()
        {
             InitializeComponent();
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Call the NavigateToRegister method of the LoginWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtblkSignUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
            if (Window.GetWindow(this) is LoginWindow mainWindow)
            {
                mainWindow.NavigateToRegister();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        private void lblblkSignUp_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            lblSignUp.Foreground = Brushes.Blue;

        }

        //----------------------------------------------------------------------------------------------------------------------------------

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (loginWorker.Login(txtUsername.Text, pwBox.Password.ToString()))
            {
                MessageBox.Show("Login Successful");
                isLogin = true;

                Login_Id = logins.GetLoginId(txtUsername.Text);
                loginInfos.AddLoginInfoUsingADO(Login_Id);

                this.closewindow();
                this.LogToApp();
               
            }
            else
            {
                MessageBox.Show("Login Failed");
                txtUsername.Clear();
                pwBox.Clear();
                isLogin = false;
                
                if (Window.GetWindow(this) is LoginWindow mainWindow)
                {
                    mainWindow.NavigateToLogin();
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Navigate to the main application    
        /// </summary>
        private void LogToApp()
        {   
            if (isLogin)
            {
                if (Window.GetWindow(this) is LoginWindow mainWindow)
                {
                    mainWindow.NavigateToApp();
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Close the LoginWindow
        /// </summary>
        private void closewindow()
        {
            if (Window.GetWindow(this) is LoginWindow mainWindow)
            {
                mainWindow.CloseWindow();
            }
        }

        
    }
}
//----------------------------------------------------------------------------------------------------------------------------------Eugene*End