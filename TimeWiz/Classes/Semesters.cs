using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TimeWiz.Classes
{
    public class Semesters : DbContext
    {

        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();

        public  Semester semeseter = new Semester();

       public ModuleTables module = new ModuleTables();
        public DbSet<Semester> Sem { get; set; }
        public Semesters ()
    {
        
    }

        private readonly string ConnectString = Properties.Settings.Default.ConnectionString;


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
            this.Connection = new SqlConnection(ConnectString);
            this.Command = new SqlCommand();
            this.Connection.ConnectionString = ConnectString;
        }


        public Semester AddSemesterAdo(int semesterNum, int numOfWeeks, string startDate, string endDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Semester (SemesterNum, NumOfWeeks, StartDate, EndDate) " +
                        "VALUES (@SemesterNum, @NumOfWeeks, @StartDate, @EndDate); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SemesterNum", semesterNum);
                        cmd.Parameters.AddWithValue("@NumOfWeeks", numOfWeeks);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        int semesterId = Convert.ToInt32(cmd.ExecuteScalar()); // Get the newly inserted semester's ID

                        if (semesterId > 0)
                        {
                            // If a semester was successfully inserted, return its details
                            return new Semester
                            {
                                Semester_Id = semesterId,
                                SemesterNum = semesterNum,
                                NumOfWeeks = numOfWeeks,
                                StartDate = Convert.ToDateTime(startDate),
                                EndDate = Convert.ToDateTime(endDate)
                            };
                        }
                        else
                        {
                            MessageBox.Show("Semester insertion failed.");
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

        public List<Semester> GetAllSemesterAdo()
        {
            List<Semester> semesters = new List<Semester>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Semester"; // SQL query to select all semesters

                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the Semester object
                                Semester semester = new Semester
                                {
                                    Semester_Id = (int)reader["Semester_Id"],
                                    SemesterNum = (int)reader["SemesterNum"],
                                    NumOfWeeks = (int)reader["NumOfWeeks"],
                                    StartDate = (DateTime)reader["StartDate"],
                                    EndDate = (DateTime)reader["EndDate"]
                                };

                                semesters.Add(semester);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return semesters;
        }


        public Semester AddSemester(int semesterNum, int numOfWeeks, string startDate, string endDate)
        {
            var semester = new Semester
            {
                SemesterNum = semesterNum,
                NumOfWeeks = numOfWeeks,
                StartDate = Convert.ToDateTime(startDate),
                EndDate = Convert.ToDateTime(endDate)
            };

            try
            {
                using (var dbContext = new Semesters()) // Replace YourDbContext with the actual name of your DbContext class
                {
                    dbContext.Sem.Add(semester);
                    dbContext.SaveChanges();
                }

                return semester; 
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public Semester UpdateSemester(int id,int semesterNum, int numOfWeeks, string startDate, string endDate)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var semester = db.Semesters.Where(s => s.Semester_Id == id).SingleOrDefault();
                if (semester != null)
                {
                    semester.SemesterNum = semesterNum;
                    semester.NumOfWeeks = numOfWeeks;
                    semester.StartDate = Convert.ToDateTime(startDate);
                    semester.EndDate = Convert.ToDateTime(endDate);
                    db.SaveChanges();
                    return semester;
                }
                else
                {
                    return null;
                }
            }
        }

        public Semester GetSemester(int id)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var semester = db.Semesters.Where(s => s.Semester_Id == id).SingleOrDefault();
                if (semester != null)
                {
                    return semester;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Semester> GetAllSemesters()
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                // Use the ToList method to retrieve all semesters as a list
                return db.Semesters.ToList();
            }
        }

      
        public Semester DeleteSemester(int id)
        {
            using (var db = new MyTimeWizDatabaseEntity())
            {
                var semester = db.Semesters.Find(id);
                if (semester != null)
                {
                    // Find modules that reference the semester and set their Semester_Id to null.
                    var modulesReferencingSemester = db.ModuleTables.Where(m => m.Semester_Id == id);
                    foreach (var module in modulesReferencingSemester)
                    {
                        module.Semester_Id = 0;
                    }

                    db.Semesters.Remove(semester);
                    db.SaveChanges();
                    return semester;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
