using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TimeWiz.Classes
{
    public class ModuleTables: DbContext
    {

        private MyTimeWizDatabaseEntities1 db = new MyTimeWizDatabaseEntities1();

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
        public ModuleTable AddModuleUsingADO(string name, string code, int credits, int semesterId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO ModuleTable (Name, Code, Credits, Semester_Id) " +
                        "VALUES (@Name, @Code, @Credits, @Semester_Id); SELECT SCOPE_IDENTITY()";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Code", code);
                        cmd.Parameters.AddWithValue("@Credits", credits);
                        cmd.Parameters.AddWithValue("@Semester_Id", semesterId);

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
                                Semester_Id = semesterId
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

        public ModuleTable AddModule(string name, string code, int credits, int semesterId)
        {
            var module = new ModuleTable
            {
                Name = name,
                Code = code,
                Credits = credits,
                Semester_Id = semesterId
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

        public ModuleTable UpdateModule(int id,string name, string code, int credits)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {


                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
                if (module != null)
                {
                    module.Name = name;
                    module.Code = code;
                    module.Credits = credits;
                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }   

        public ModuleTable GetModule(int id)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
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
        public List <ModuleTable> GetAllModules()
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var modules = db.ModuleTables.ToList();
                if (modules != null)
                {
                    return modules;
                }
                else
                {
                    return null;
                }
            }
        }
        public ModuleTable DeleteModule(int id)
        {
            using (db = new MyTimeWizDatabaseEntities1())
            {
                var module = db.ModuleTables.Where(m => m.Module_Id == id).SingleOrDefault();
                if (module != null)
                {
                    db.ModuleTables.Remove(module);
                    db.SaveChanges();
                    return module;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
