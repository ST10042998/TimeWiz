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
        /// Holds the semester num
        /// </summary>
        private int semesterNum;
        public int SemesterNum { get => semesterNum; set => semesterNum = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the start date of the semester 
        /// </summary>
        private string startDate;
        public string StartDate { get => startDate; set => startDate = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the end date of the semester 
        /// </summary>
        private string endDate;
        public string EndDate { get => endDate; set => endDate = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Default constructor
        /// </summary>
        public SemesterClass()
        {
         
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..