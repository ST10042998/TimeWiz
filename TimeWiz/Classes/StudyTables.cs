using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace TimeWiz.Classes
{
    public class StudyTables
    {
        private MyTimeWizDatabaseEntities1 db = new MyTimeWizDatabaseEntities1();

        private StudyTable study = new StudyTable();

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

        public StudyTables()
        {

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void ConnectDB()
        {
            this.Connection = new SqlConnection(ConnectString);
            this.Command = new SqlCommand();
            this.Connection.ConnectionString = ConnectString;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public StudyTable AddStudyADO(int classHoursPerWeek, int selfStudyHours, int studiedHours, int remainingWeekHours, int module_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO StudyTable (ClassHoursPerWeek, SelfStudyHours, StudiedHours, RemainingWeekHours, Module_Id) VALUES (@ClassHoursPerWeek, @SelfStudyHours, @StudiedHours, @RemainingWeekHours, @Module_Id)", connection);
                    cmd.Parameters.AddWithValue("@ClassHoursPerWeek", classHoursPerWeek);
                    cmd.Parameters.AddWithValue("@SelfStudyHours", selfStudyHours);
                    cmd.Parameters.AddWithValue("@StudiedHours", studiedHours);
                    cmd.Parameters.AddWithValue("@RemainingWeekHours", remainingWeekHours);
                    cmd.Parameters.AddWithValue("@Module_Id", module_id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return study;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public StudyTable AddStudy(int classHoursPerWeek, int selfStudyHours, int studiedHours, int remainingWeekHours, int module_id)
        {
            var study = new StudyTable
            {
                ClassHoursPerWeek = classHoursPerWeek,
                SelfStudyHours = selfStudyHours,
                StudiedHours = studiedHours,
                RemainingWeekHours = remainingWeekHours,
                Module_Id = module_id
            };
            try
            {
                using (db = new MyTimeWizDatabaseEntities1())
                {
                    db.StudyTables.Add(study);
                    db.SaveChanges();
                    if (study != null)
                    {
                        return study;
                    }
                    else
                    {
                        return null;
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

        public StudyTable UpdateStudy(int id, int classHoursPerWeek, int selfStudyHours, int studiedHours, int remainingWeekHours, int module_id, decimal progressBarPercentage, DateTime studyDate)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var study = db.StudyTables.Where(s => s.Study_Id == id).SingleOrDefault();
                study.ClassHoursPerWeek = classHoursPerWeek;
                study.SelfStudyHours = selfStudyHours;
                study.StudiedHours = studiedHours;
                study.RemainingWeekHours = remainingWeekHours;
                study.Module_Id = module_id;
                study.ProgressBarPercentage = progressBarPercentage;
                study.StudyDate = studyDate;
                db.SaveChanges();
                if (study != null)
                {
                    return study;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public StudyTable GetStudy(int id)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var study = db.StudyTables.Where(s => s.Study_Id == id).SingleOrDefault();
                if (study != null)
                {
                    return study;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public List<StudyTable> GetAllStudyTables()
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                // Use the ToList method to retrieve all semesters as a list
                return db.StudyTables.ToList();
            }
        }
    }

}
