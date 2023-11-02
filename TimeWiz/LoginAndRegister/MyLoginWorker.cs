using MyTimeWizClassLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using TimeWiz.Classes;

namespace TimeWiz.LoginAndRegister
{
    public class MyLoginWorker
    {

        private string pass = string.Empty;
      
        Students student = new Students();
        Logins login = new Logins();

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Default constructor
        /// </summary>
        public MyLoginWorker()
        {
         
        }   

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Add a new student to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="gender"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddStudent(string name, string surname, string email, string gender, string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) && this.CheckPassword(password))
            {
                this.HashPassword(password);
                login.AddLoginUsingAdo(username, pass);
                
            }

            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(surname) && !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(gender) && this.CheckEmail(email) )
            {
                student.AddStudentUsingADO(name, surname, email, gender, login.GetLoginId(username));
            }
          
            else
            {
                return;
            }
        }
          
        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Hash the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return pass = Encoding.ASCII.GetString(data);
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// CHECK PASSWORD
        /// </summary>
        /// <param name="passwd"></param>
        /// <returns></returns>
        public bool CheckPassword(string passwd)
        {
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasSpecialChar = false;

            // Check the password length
            if (passwd.Length < 8 || passwd.Length > 14)
            {
                MessageBox.Show("Password must be between 8 and 14 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Check for uppercase, lowercase, and special characters
            foreach (char ch in passwd)
            {
                if (char.IsUpper(ch))
                {
                    hasUpperCase = true;
                }
                else if (char.IsLower(ch))
                {
                    hasLowerCase = true;
                }
                else if ("%!@#$%^&*()?/>.<,:;'\\|}]{[_~`+=-\"".Contains(ch))
                {
                    hasSpecialChar = true;
                }

                // If all criteria are met, return true
                if (hasUpperCase && hasLowerCase && hasSpecialChar)
                {
                    return true;
                }
            }
            MessageBox.Show("Password must contain at least one uppercase letter, one lowercase letter, and one special character.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;

        }

        //----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// log into the appliaction if the login is successful
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter a username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using (var db = new MyTimeWizDatabaseEntity())
            {
                var login = db.Logins.FirstOrDefault(l => l.UserName == username);

                if (login != null && login.Password == this.HashPassword(password))
                {

                    return true;
                }
            }
            MessageBox.Show("Login failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        //----------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// check if the email is valid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool CheckEmail(string email)
        {
           if( email.Contains("@"))
            {
                return true;
            }
           else
            {
                MessageBox.Show("Email is not valid","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
        }
}
}
//----------------------------------------------------------------------------------------------------------------------------------Eugene*End