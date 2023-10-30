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

        private MyTimeWizDatabaseEntities1 db = new MyTimeWizDatabaseEntities1();

        private Semester semeseter = new Semester();
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
            using (db = new MyTimeWizDatabaseEntities1())
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
            using (db = new MyTimeWizDatabaseEntities1())
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
            using (db = new MyTimeWizDatabaseEntities1())
            {
                // Use the ToList method to retrieve all semesters as a list
                return db.Semesters.ToList();
            }
        }

        public Semester DeleteSemester(int id)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var semester = db.Semesters.Where(s => s.Semester_Id == id).SingleOrDefault();
                if (semester != null)
                {
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
