using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWiz.Classes
{
    public class LoginInfos
    {

        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();

        private LoginInfo logInfo = new LoginInfo();


        private string connectionString = Properties.Settings.Default.ConnectionString;

        public LoginInfos()
        {

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// adding login info 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoginInfo AddLoginInfoUsingADO(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LoginInfo (Login_Id)" +
                                               "VALUES (@Login_Id)", connection))
                    {
                        cmd.Parameters.AddWithValue("@Login_Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return logInfo;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// delete login info
        /// </summary>
        public void DeleteLoginInfo()
        {
          
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM LoginInfo", connection))
                    {
                       cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
                      
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// get last added login info
        /// </summary>
        /// <returns></returns>
        public LoginInfo GetLastAdded()
        {
            using (var db = new MyTimeWizDatabaseEntity())
            {
                var lastAdded = db.LoginInfoes.OrderByDescending(l => l.Login_Id).FirstOrDefault();
                if (lastAdded != null)
                {
                    return lastAdded;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------Eugene*end