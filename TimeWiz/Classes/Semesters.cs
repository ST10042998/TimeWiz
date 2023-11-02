﻿using System;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public Semesters ()
        {
        
        }

        //connection string
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// add semester using ado
        /// </summary>
        /// <param name="semesterNum"></param>
        /// <param name="numOfWeeks"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="student_id"></param>
        /// <returns></returns>
        public Semester AddSemesterAdo(int semesterNum, int numOfWeeks, string startDate, string endDate, int student_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Semester (SemesterNum, NumOfWeeks, StartDate, EndDate, Student_Id) " +
                        "VALUES (@SemesterNum, @NumOfWeeks, @StartDate, @EndDate, @Student_Id); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SemesterNum", semesterNum);
                        cmd.Parameters.AddWithValue("@NumOfWeeks", numOfWeeks);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@Student_Id", student_id);

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
                                EndDate = Convert.ToDateTime(endDate),
                                Student_Id = student_id
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all semesters using ado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Semester> GetAllSemesterAdo(int id)
        {
            List<Semester> semesters = new List<Semester>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Semester WHERE Student_Id = @Student_Id";

                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Student_Id", id);

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
                                    EndDate = (DateTime)reader["EndDate"],
                                    Student_Id = (int)reader["Student_Id"]
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding semester using entity framework
        /// </summary>
        /// <param name="semesterNum"></param>
        /// <param name="numOfWeeks"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="student_id"></param>
        /// <returns></returns>
        public Semester AddSemester(int semesterNum, int numOfWeeks, string startDate, string endDate, int student_id)
        {
            var semester = new Semester
            {
                SemesterNum = semesterNum,
                NumOfWeeks = numOfWeeks,
                StartDate = Convert.ToDateTime(startDate),
                EndDate = Convert.ToDateTime(endDate),
                Student_Id = student_id
            };

            try
            {
                using (var dbContext = new Semesters()) 
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// update semester using entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="semesterNum"></param>
        /// <param name="numOfWeeks"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get semester using entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all semesters using entity
        /// </summary>
        /// <returns></returns>
        public List<Semester> GetAllSemesters()
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
               
                return db.Semesters.ToList();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
      
        /// <summary>
        /// delete semester using entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end