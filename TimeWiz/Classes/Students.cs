using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TimeWiz.Classes
{
    public class Students
    {

        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();

        private Student student = new Student();

        //connection string
        private string connectionString = Properties.Settings.Default.ConnectionString;
       

        /// <summary>
        /// 
        /// </summary>
        private SqlConnection Connection;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        private SqlCommand Command;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public void ConnectDB()
        {
            this.Connection = new SqlConnection(connectionString);
            this.Command = new SqlCommand();
            this.Connection.ConnectionString = connectionString;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// constructor
        /// </summary>
        public Students()
        {

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
     
        /// <summary>
        /// adding student to database using ADO
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="gender"></param>
        /// <param name="login_id"></param>
        /// <returns></returns>
        public Student AddStudentUsingADO(string name, string surname, string email, string gender, int login_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Student (Name, Surname, Email, Gender, Login_Id)" +
                        " VALUES (@Name, @Surname, @Email, @Gender, @Login_Id); SELECT SCOPE_IDENTITY()", connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Surname", surname);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Login_Id", login_id);

                        int studentId = Convert.ToInt32(cmd.ExecuteScalar()); // Get the newly inserted module's ID

                        if (studentId > 0)
                        {
                            // If a module was successfully inserted, return its details
                            return new Student
                            {
                                Student_Id = studentId,
                                Name = name,
                                Surname = surname,
                                Email = email,
                                Gender = gender,
                                Login_Id = login_id
                            };
                        }
                        else
                        {
                            MessageBox.Show("Student insertion failed.");
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding student to database using Entity Framework
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        /// <param name="gender"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public Student AddStudent(string name,string surname,string email,string gender,int login)
        {

            var student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                Gender = gender,
                Login_Id = login
            };
            try

            {
                using (var dbContext = new MyTimeWizDatabaseEntity())
                {
                    dbContext.Students.Add(student);
                    dbContext.SaveChanges();
                }

                return student;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("Student insertion failed.");

                return null;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// getting student id using ADO
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public int GetStudentId(int login)
        {
            int id = 0;
            string query = "SELECT Student_Id FROM Student WHERE Login_Id = @Login_Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Login_Id", login);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(0); // Assuming Student_Id is an integer field.
                        }
                    }
                }
            }

            return id;
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
