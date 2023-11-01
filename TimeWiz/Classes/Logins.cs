using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWiz.Classes
{
    public class Logins
    {

        private MyTimeWizDatabaseEntity db = new MyTimeWizDatabaseEntity();

        private Login login = new Login();

        public Logins()
        {

        }
       
        public Login AddLoginData(string username, string password)
        {
            using(var db = new MyTimeWizDatabaseEntity())
            {
                var login = new Login
                {
                    UserName = username,
                    Password = password
                };

                try
                {
                    db.Logins.Add(login);
                    db.SaveChanges();
                    if (login != null)
                    {
                        return login;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

    }
}
