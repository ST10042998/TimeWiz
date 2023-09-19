using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTimeWizClassLib
{
    public class StudyClass
    {
        /// <summary>
        /// Creating obj for my SemesterDataV
        /// </summary>
        public SemesterDataClass semesterData = new SemesterDataClass();
      
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// list of semesterclass
        /// </summary>
        public List<SemesterDataClass> SemesterList;

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Default constructor
        /// </summary>
        public StudyClass()
        {
            SemesterList = new List<SemesterDataClass>();
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..
