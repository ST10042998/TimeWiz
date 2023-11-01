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

        public Students()
        {

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public Student AddStudentUsingADO(string name, string surname, string email, string gender)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Student (Name, Surname, Email, Gender)" +
                        " VALUES (@Name, @Surname, @Email, @Gender); SELECT SCOPE_IDENTITY()", connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Surname", surname);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Gender", gender);

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
                                Gender = gender
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

        public Student AddStudent(string name,string surname,string email,string gender)
        {

            var student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                Gender = gender
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


    
        
    }
}
