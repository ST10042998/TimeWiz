using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace TimeWiz.Classes
{
    public class ModuleTables : DbContext
    {

        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();

        private ModuleTable module = new ModuleTable();
        public DbSet<ModuleTable> mod { get; set; }

        /// <summary>
        /// connection string
        /// </summary>
        private string connectionString = Properties.Settings.Default.ConnectionString;
        public ModuleTables()
        {

        }

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
        /// connect to database
        /// </summary>
        public void ConnectDB()
        {
            this.Connection = new SqlConnection(connectionString);
            this.Command = new SqlCommand();
            this.Connection.ConnectionString = connectionString;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// add module to database using ado.net   
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="credits"></param>
        /// <param name="semesterId"></param>
        /// <param name="classhours"></param>
        /// <param name="selfstudyhours"></param>
        /// <returns></returns>
        public ModuleTable AddModuleUsingADO(string name, string code, int credits, int semesterId, int classhours, int selfstudyhours)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO ModuleTable (Name, Code, Credits, Semester_Id, ClassHoursPerWeek, SelfStudyHours, RemainingWeekHours, ProgressBarPercentage, StudyDate, StudiedHours)" +
                        " VALUES (@Name, @Code, @Credits, @Semester_Id, @ClassHoursPerWeek, @SelfStudyHours, @RemainingWeekHours, @ProgressBarPercentage, @StudyDate, @StudiedHours); SELECT SCOPE_IDENTITY()", connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Code", code);
                        cmd.Parameters.AddWithValue("@Credits", credits);
                        cmd.Parameters.AddWithValue("@Semester_Id", semesterId);
                        cmd.Parameters.AddWithValue("@ClassHoursPerWeek", classhours);
                        cmd.Parameters.AddWithValue("@SelfStudyHours", selfstudyhours);
                        cmd.Parameters.AddWithValue("@RemainingWeekHours", DBNull.Value); // Nullable column
                        cmd.Parameters.AddWithValue("@ProgressBarPercentage", 0); // Nullable column
                        cmd.Parameters.AddWithValue("@StudyDate", DBNull.Value); // Nullable column
                        cmd.Parameters.AddWithValue("@StudiedHours", DBNull.Value); // Nullable column

                        int moduleId = Convert.ToInt32(cmd.ExecuteScalar()); // Get the newly inserted module's ID

                        if (moduleId > 0)
                        {
                            // If a module was successfully inserted, return its details
                            return new ModuleTable
                            {
                                Module_Id = moduleId,
                                Name = name,
                                Code = code,
                                Credits = credits,
                                Semester_Id = semesterId,
                                ClassHoursPerWeek = classhours,
                                SelfStudyHours = selfstudyhours
                            };
                        }
                        else
                        {
                            MessageBox.Show("Module insertion failed.");
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
        /// add module to database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="credits"></param>
        /// <param name="semesterId"></param>
        /// <param name="classhours"></param>
        /// <param name="selfstudy"></param>
        /// <returns></returns>
        public ModuleTable AddModule(string name, string code, int credits, int semesterId, int classhours, int selfstudy)
        {
            var module = new ModuleTable
            {
                Name = name,
                Code = code,
                Credits = credits,
                Semester_Id = semesterId,
                ClassHoursPerWeek = classhours,
                SelfStudyHours = selfstudy
            };
            try

            {
                using (var dbContext = new ModuleTables())
                {
                    dbContext.mod.Add(module);
                    dbContext.SaveChanges();
                }

                return module;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                MessageBox.Show("Module insertion failed.");

                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all modules from database using ado.net
        /// </summary>
        /// <param name="selectedSemesterNum"></param>
        /// <returns></returns>
        public List<ModuleTable> GetAllModulesAdo(string selectedSemesterNum)
        {
            List<ModuleTable> modules = new List<ModuleTable>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = @" SELECT *
                                            FROM ModuleTable
                                            JOIN Semester ON ModuleTable.Semester_Id = Semester.Semester_Id
                                            WHERE Semester.SemesterNum = @selectedSemesterNum";


                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@selectedSemesterNum", selectedSemesterNum);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database columns to the ModuleTable object
                                ModuleTable module = new ModuleTable
                                {
                                    Module_Id = (int)reader["Module_Id"],
                                    Name = (string)reader["Name"],
                                    Code = (string)reader["Code"],
                                    Credits = (int)reader["Credits"]
                                };

                                modules.Add(module);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

            return modules;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// update module using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="credits"></param>
        /// <param name="semester_id"></param>
        /// <param name="classHours"></param>
        /// <param name="selfHours"></param>
        /// <param name="remainingHours"></param>
        /// <param name="Progressbar"></param>
        /// <param name="date"></param>
        /// <param name="studiedHrs"></param>
        /// <returns></returns>
        public ModuleTable UpdateModule(int id, string name, string code, int credits, int semester_id, int classHours, int selfHours, int remainingHours, int Progressbar, DateTime date, int studiedHrs)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {


                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
                if (module != null)
                {
                    module.Name = name;
                    module.Code = code;
                    module.Credits = credits;
                    module.Semester_Id = semester_id;
                    module.ClassHoursPerWeek = classHours;
                    module.SelfStudyHours = selfHours;
                    module.RemainingWeekHours = remainingHours;
                    module.ProgressBarPercentage = Progressbar;
                    module.StudyDate = date;
                    module.StudiedHours = studiedHrs;

                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// updating module ,study hours part using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remainingHours"></param>
        /// <param name="Progressbar"></param>
        /// <param name="date"></param>
        /// <param name="studiedHrs"></param>
        /// <returns></returns>
        public ModuleTable UpdateStudyModule(int id, int remainingHours, int Progressbar, DateTime date, int studiedHrs)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {


                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
                if (module != null)
                {

                    module.RemainingWeekHours = remainingHours;
                    module.ProgressBarPercentage = Progressbar;
                    module.StudyDate = date;
                    module.StudiedHours = studiedHrs;

                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// update module using ado.net
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remainingHours"></param>
        /// <param name="Progressbar"></param>
        /// <param name="date"></param>
        /// <param name="studiedHrs"></param>
        /// <returns></returns>
        public ModuleTable UpdateStudyModuleAdo(int id, int remainingHours, int Progressbar, DateTime date, int studiedHrs)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE ModuleTable " +
                                    "SET RemainingWeekHours = @RemainingWeekHours, " +
                                    "ProgressBarPercentage = @ProgressBarPercentage, " +
                                    "StudyDate = @StudyDate, " +
                                    "StudiedHours = @StudiedHours " +
                                    "WHERE Module_Id = @Module_Id";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@RemainingWeekHours", remainingHours);
                    cmd.Parameters.AddWithValue("@ProgressBarPercentage", Progressbar);
                    cmd.Parameters.AddWithValue("@StudyDate", date);
                    cmd.Parameters.AddWithValue("@StudiedHours", studiedHrs);
                    cmd.Parameters.AddWithValue("@Module_Id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        return GetModuleByIdAdo(id);
                    }
                    else
                    {
                        // Module with the specified ID was not found
                        return null;
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get study hours 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetStudiedHours(int id)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
                if (module != null)
                {
                    id = module.StudiedHours.Value;
                    return id;
                }
                else
                {
                    return 0;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get module by modulecode
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<ModuleTable> GetModuleByCode(string code)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var module = db.ModuleTables.Where(m => m.Code == code).ToList();
                if (module != null)
                {
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get all modules from database that are in the selected semester
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public List<ModuleTable> GetAllModules(Semester semester)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var modules = db.ModuleTables.Where(m => m.Semester_Id == semester.Semester_Id).ToList();
                return modules;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// delete module by semester id using entity framework
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModuleTable> DeleteModuleBySemesterId(int id)
        {
            using (db = new MyTimeWizDatabaseEntity())
            {
                var module = db.ModuleTables.Where(m => m.Semester_Id == id).ToList();
                if (module.Count > 0)
                {
                    db.ModuleTables.RemoveRange(module);
                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return module;
                }
            }

        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get student id using login id
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
                            id = reader.GetInt32(0);
                        }
                    }
                }
            }

            return id;
        }

        public ModuleTable GetModuleByIdAdo(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM ModuleTable WHERE Module_Id = @Module_Id";

                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Module_Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Map the database columns to the ModuleTable object
                            ModuleTable module = new ModuleTable
                            {
                                Module_Id = (int)reader["Module_Id"],
                                // Map other properties as needed
                            };

                            return module;
                        }
                        else
                        {
                            // No record found with the specified Module_Id
                            return null;
                        }
                    }
                }
            }
        }

    }
}
//------------------------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*end
