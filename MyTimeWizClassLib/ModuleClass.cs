using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTimeWizClassLib
{
    public class ModuleClass
    {
        /// <summary>
        /// Holds the module code
        /// </summary>
        private string code;
        public string Code { get => code; set => code = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------
      
        /// <summary>
        /// Holds the module name
        /// </summary>
        private string name;
        public string Name { get => name; set => name = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the module credits
        /// </summary>
        private int credits;
        public int Credits { get => credits; set => credits = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the class hours per week for the module
        /// </summary>
        private int classHoursPerWeek;
        public int ClassHoursPerWeek { get => classHoursPerWeek; set => classHoursPerWeek = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Holds the self study hours of the module
        /// </summary>
        private int selfStudyHours;
        public int SelfStudyHours { get => selfStudyHours; set => selfStudyHours = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the number of hours studied for this module
        /// </summary>
        private int? studiedHours;
        public int? StudiedHours
        {
            get { return studiedHours; }
            set
            {
                // Update the total studied hours when setting the value.
                if (value.HasValue)
                    TotalStudiedHours += value.Value;

                studiedHours = value;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Accumulated studied hours
        /// </summary>
        public int? TotalStudiedHours { get; private set; }

    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..