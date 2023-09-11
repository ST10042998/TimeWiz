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
        ModuleClass module = new ModuleClass();
        SemesterClass semester = new SemesterClass();
        /// <summary>
        /// 
        /// </summary>
        private int hourStudied = 0;
        public int HourStudied { get => hourStudied; set => hourStudied = value; }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        public List<SemesterClass> Semester = new List<SemesterClass>(); 

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Dictionary that holds my selfstudyhours 
        /// </summary>
        public Dictionary<string, string> StudyHours = new Dictionary<string, string>();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// calculating the self study amount of time for the student (selfstudy = number of credits * 10 / number of weeks - class hours per week)
        /// </summary>
        /// <param name="credits"></param>
        /// <param name="weeks"></param>
        /// <param name="classHours"></param>
        /// <returns></returns>
        public int CalculateSelfStudyHours(string MCode, int NumberOfWeeks, int ClassHoursPerWeek, int Credits)
        {
            var studyHours = 0;
            var moduleCode = MCode;

            try
            {

                if (NumberOfWeeks > ClassHoursPerWeek)
                {
                    studyHours = Credits *10 / (NumberOfWeeks - ClassHoursPerWeek);
                }
                else if (studyHours == 0)
                {
                    // Handle the case where division by zero would occur

                    MessageBox.Show($"Warning: Division by zero prevented for module {moduleCode}.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Store self-study hours in a dictionary
            StudyHours[moduleCode] = studyHours.ToString();

            return studyHours;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculating the remaining study hours for module
        /// </summary>
        public void RemainingStudyHours()
        { 
            foreach (var moduleCode in StudyHours.Keys)
            {
                if (StudyHours.ContainsKey(moduleCode))
                {
                    // Use LINQ to find the ModuleClass instance based on the module code.
                    ModuleClass module = semester.ModuleList.FirstOrDefault(m => m.Code == moduleCode);

                    // Deduct the calculated self-study hours from the total self-study hours.
                    module.StudiedHours =- module.SelfStudyHours ;
                }
            }
        }


    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..