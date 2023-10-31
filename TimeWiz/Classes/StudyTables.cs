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

        public StudyTable AddStudyADO(int studiedHours, int remainingWeekHours, int module_id, decimal progressbar, DateTime studyDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO StudyTable ( StudiedHours, RemainingWeekHours, Module_Id, ProgressBarPercentage, StudyDate) VALUES (@ClassHoursPerWeek, @SelfStudyHours, @StudiedHours, @RemainingWeekHours, @Module_Id, @ProgressBarPercentage, @StudyDate)", connection);
                   
                    cmd.Parameters.AddWithValue("@StudiedHours", studiedHours);
                    cmd.Parameters.AddWithValue("@RemainingWeekHours", remainingWeekHours);
                    cmd.Parameters.AddWithValue("@Module_Id", module_id);
                    cmd.Parameters.AddWithValue("@ProgressBarPercentage", progressbar);
                    cmd.Parameters.AddWithValue("@StudyDate", studyDate);
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

        public StudyTable AddStudy( int studiedHours, int remainingWeekHours, int module_id, decimal progressbar,DateTime studyDate)
        {
            var study = new StudyTable
            {
                StudiedHours = studiedHours,
                RemainingWeekHours = remainingWeekHours,
                Module_Id = module_id,
                ProgressBarPercentage = progressbar,
                StudyDate = studyDate   

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

        public List <StudyTable> GetStudyByID(int id)
        {
                using (db = new MyTimeWizDatabaseEntities1())
                {
                    var study = db.StudyTables.Where(st => st.Module_Id == id).ToList();
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

        public StudyTable UpdateStudy(int id, int studiedHours, int remainingWeekHours, int module_id, decimal progressBarPercentage, DateTime studyDate)
        {
            using (var db = new MyTimeWizDatabaseEntities1())
            {
                var study = db.StudyTables.Where(s => s.Module_Id == id).SingleOrDefault();
                if (study != null)
                {
                   
                    study.StudiedHours = studiedHours;
                    study.RemainingWeekHours = remainingWeekHours;
                    study.Module_Id = module_id;
                    study.ProgressBarPercentage = progressBarPercentage;
                    study.StudyDate = studyDate;
                    db.SaveChanges();
                }
                return study;
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
