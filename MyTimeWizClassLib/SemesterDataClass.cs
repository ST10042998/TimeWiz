using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimeWizClassLib
{
   public class SemesterDataClass
    {
        /// <summary>
        /// creating a obj for my semesterclass
        /// </summary>
        public SemesterClass semester = new SemesterClass();

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// creating a obj for my moduleclass
        /// </summary>
        public ModuleClass module = new ModuleClass();

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// List of the module class
        /// </summary>
        public List<ModuleClass> ModuleList = new List<ModuleClass>();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// list of semesterclass
        /// </summary>
        public List<SemesterClass> SemesterList = new List<SemesterClass>();

    }
}
