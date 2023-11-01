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

namespace TimeWiz.LoginAndRegister
{
    /// <summary>
    /// Interaction logic for MyRegister.xaml
    /// </summary>
    public partial class MyRegister : UserControl
    {
        StudentClass studentClass = new StudentClass();
        MyLoginWorker loginWorker = new MyLoginWorker();
        public MyRegister()
        {
            InitializeComponent();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StudentClass student  = new StudentClass();
            student.Name = txtName.Text;
            student.Surname = txtSurname.Text;
            student.Email = txtEmail.Text;
            if(radioFemale.IsChecked == true)
            {
                student.Gender = radioFemale.Content.ToString();
            }
            else if(radioMale.IsChecked == true)
            {
                student.Gender = radioMale.Content.ToString();
            }
            student.UserName = txtUsername.Text;
            student.Password = pwBox.Password.ToString();
            MessageBox.Show($"{student.Password}");
            MessageBox.Show($"{student.Gender}");

            loginWorker.AddStudent(student.Name,student.Surname,student.Email,student.Gender,student.UserName,student.Password);

            if(loginWorker.CheckPassword(student.Password) == true)
            {
                this.navigateToLogin();
            }
            else
            {
               pwBox.Clear();
            }
        }

        private void navigateToLogin()
        {
            if (Window.GetWindow(this) is LoginWindow mainWindow)
            {
                mainWindow.NavigateToLogin();
            }
        }
    }
}
