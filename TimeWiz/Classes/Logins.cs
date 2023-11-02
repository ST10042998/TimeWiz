using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Windows;
using System.Drawing.Design;

namespace TimeWiz.Classes
{
    public class Logins
    {
        //creating instance of database
        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();
        private Login login = new Login();
        private string connectionString = Properties.Settings.Default.ConnectionString;
        //--------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// constructor
        /// </summary>
        public Logins()
        {

        }
       
        //--------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// add login data to database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Login AddLoginData(string username, string password)
        {
            using(var db = new MyTimeWizDatabaseEntity())
            {
                var login = new Login
                {
                    UserName = username,
                    Password = password
                };

                try
                {
                    db.Logins.Add(login);
                    db.SaveChanges();
                    if (login != null)
                    {
                        return login;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding login data using ado.net
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
       public Login AddLoginUsingAdo(string username,string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Login ( UserName, Password)" +
                        " VALUES (@UserName, @Password); SELECT SCOPE_IDENTITY()", connection))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int Login_id = Convert.ToInt32(cmd.ExecuteScalar()); // Get the newly inserted module's ID

                        if (Login_id > 0)
                        {
                            // If a module was successfully inserted, return its details
                            return new Login
                            {
                                
                                UserName = username,
                                Password = password,
                               
                            };
                        }
                        else
                        {
                            MessageBox.Show("Login insertion failed.");
                            return null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get login data using ado.net
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetLoginId(string username)
        {
            int id = 0;
            string query = "SELECT Login_Id FROM Login WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(0); 
                        }
                    }
                }
            }

            return id;
        }


    }
}
//----------------------------------------------------------------------------------------------------------------------------------Eugene*End..