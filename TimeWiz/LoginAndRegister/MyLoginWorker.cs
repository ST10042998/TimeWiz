using System;
using System.Collections.Generic;
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

        public void AddStudent(string name, string surname, string email, string gender, string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(surname) && !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(gender))
            {
                student.AddStudentUsingADO(name, surname, email, gender);
            }
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) && this.CheckPassword(password))
            {
                this.HashPassword(password);
                login.AddLoginData(username, pass);
            }
            else
            {
                return;
            }
        }
          
        public string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return pass = Encoding.ASCII.GetString(data);
        }

        public bool CheckPassword(string passwd)
        {
            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasSpecialChar = false;

            // Check the password length
            if (passwd.Length < 8 || passwd.Length > 14)
            {
                MessageBox.Show("Password must be between 8 and 14 characters.");
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
            MessageBox.Show("Password must contain at least one uppercase letter, one lowercase letter, and one special character.");
            return false;

        }

        public bool Login(string username, string password)
        {
            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password))
            {
                var db = new MyTimeWizDatabaseEntity();
                var login = db.Logins.FirstOrDefault(l => l.UserName == username);
                if (login != null)
                {
                    if (login.Password == this.HashPassword(password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                return false;
            }
        }

    }
}
