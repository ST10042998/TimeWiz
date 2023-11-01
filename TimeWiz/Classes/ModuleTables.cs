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
    public class ModuleTables: DbContext
    {

        private MyTimeWizDatabaseEntities2 db = new MyTimeWizDatabaseEntities2();

        private ModuleTable module = new ModuleTable();
        public DbSet<ModuleTable> mod { get; set; }

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
        /// 
        /// </summary>
        public void ConnectDB()
        {
            this.Connection = new SqlConnection(connectionString);
            this.Command = new SqlCommand();
            this.Connection.ConnectionString = connectionString;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
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
                        cmd.Parameters.AddWithValue("@ProgressBarPercentage", 0 ); // Nullable column
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


        public ModuleTable AddModule(string name, string code, int credits, int semesterId ,int classhours , int selfstudy)
        {
            var module = new ModuleTable
            {
                Name = name,
                Code = code,
                Credits = credits,
                Semester_Id = semesterId,
                ClassHoursPerWeek  = classhours,
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
                throw;
                MessageBox.Show(e.Message);
            }

            return modules;
        }

        
        public ModuleTable UpdateModule(int id,string name, string code, int credits,int semester_id,int classHours, int selfHours, int remainingHours,int Progressbar, DateTime date, int studiedHrs)
        {
            using (db = new MyTimeWizDatabaseEntities2())
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

        public ModuleTable UpdateStudyModule(int id, int remainingHours, int Progressbar, DateTime date,int studiedHrs)
        {
            using (db = new MyTimeWizDatabaseEntities2())
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

        public int GetStudiedHours(int id)
        {
            using (db = new MyTimeWizDatabaseEntities2())
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

        public List<ModuleTable> GetModuleByCode(string code)
        {
            using (db = new MyTimeWizDatabaseEntities2())
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
        public List<ModuleTable> GetAllModules(Semester semester)
        {
            using (db = new MyTimeWizDatabaseEntities2())
            {
                var modules = db.ModuleTables.Where(m => m.Semester_Id == semester.Semester_Id).ToList();
                return modules;
            }
        }
     

        public List <ModuleTable> DeleteModuleBySemesterId(int id)
        {
            using (db = new MyTimeWizDatabaseEntities2())
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

    }
}
