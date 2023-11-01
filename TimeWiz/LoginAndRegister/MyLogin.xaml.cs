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

namespace TimeWiz.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for MyLogin.xaml
    /// </summary>
    public partial class MyLogin : UserControl
    {
        MyLoginWorker loginWorker = new MyLoginWorker();
        public MyLogin()
        {
            InitializeComponent();
        }


        private void txtblkSignUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Call the NavigateToRegister method of the LoginWindow
            if (Window.GetWindow(this) is Login mainWindow)
            {
                mainWindow.NavigateToRegister();
            }
        }

        private void lblblkSignUp_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            lblSignUp.Foreground = Brushes.Blue;

        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {   
            MessageBox.Show($"{pwBox.Password.ToString()}");
          if (loginWorker.Login(txtUsername.Text, pwBox.Password.ToString()))
            {
                MessageBox.Show("Login Successful");
            }
            else
            {
                MessageBox.Show("Login Failed");
                txtUsername.Clear();
                pwBox.Clear();
            }
        }
    }
}
