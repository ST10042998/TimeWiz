using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimeWizClassLib
{
   public class SemesterClass
    {
        /// <summary>
        /// Holds the number of weeks the semester has
        /// </summary>
        private int numberOfWeeks;
        public int NumberOfWeeks { get => numberOfWeeks; set => numberOfWeeks = value; }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the start date of the semester 
        /// </summary>
        private DateTime startDate;
        public DateTime StartDate { get => startDate; set => startDate = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
       
        /// <summary>
        /// List of the module class
        /// </summary>
        public List<ModuleClass> ModuleList = new List<ModuleClass>();
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..