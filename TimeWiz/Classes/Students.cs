using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TimeWiz.Classes
{
    public class Students
    {

        private MyTimeWizDatabaseEntities1 db = new MyTimeWizDatabaseEntities1();

        private Student student = new Student();

        public Students()
        {

        }


        public Student AddStudent(string name, string surname, string email, string gender)
        {
            var student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                Gender = gender
            };

            try
            {
                db.Students.Add(student);
                db.SaveChanges();
                if (student != null)
                {
                    return student;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
