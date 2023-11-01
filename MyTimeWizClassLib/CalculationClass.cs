using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace MyTimeWizClassLib
{
   public class CalculationClass
    {

        /// <summary>
        /// Calculates the end date of the semester
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>
        public string CalculateEndOfSemester(DateTime startDate, int numberOfWeeks)
        {
            // Calculate the end date by adding the number of weeks to the start date
           string endDate = Convert.ToString(startDate.AddDays(numberOfWeeks * 7));

            return endDate;
        }

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
                    studyHours = Credits * 10 / NumberOfWeeks - ClassHoursPerWeek;
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

            return studyHours;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method that calculates the remaining hours for current week
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public int CalculateRemainingHoursForCurrentWeek(Dictionary<DateTime, int> StudiedHoursPerDate, int SelfStudyHours)
        {
            DateTime currentDate = DateTime.Now.Date;
            int currentWeek = GetWeekOfYear(currentDate);

            // Calculate the start and end dates of the current week
            DateTime startOfWeek = FirstDateOfWeek(currentDate.Year, currentWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6); // Assuming you have a 6-day study week

            int totalStudiedHoursThisWeek = StudiedHoursPerDate
                .Where(entry => entry.Key >= startOfWeek && entry.Key <= endOfWeek)
                .Sum(entry => entry.Value);

            int remainingHours = SelfStudyHours - totalStudiedHoursThisWeek;
            return remainingHours;
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// ProgressBar calculation that will give percentage
        /// </summary>
        /// <returns></returns>
        public double ProgressBarCal(Dictionary<DateTime, int> StudiedHoursPerDate, int selfStudyHours)
        {
            double progress = 0.0; // Use double to store a fractional result

            foreach (var entry in StudiedHoursPerDate)
            {
                progress += ((double)entry.Value / selfStudyHours) * 100.0;
            }

            return progress;
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// calculate the week im in now based on the number of weeks of a semester
        /// </summary>
        /// <param name="numberOfWeeks"></param>
        /// <returns></returns>
        public int GetCurrentWeek(DateTime startDate, int numberOfWeeks)
        {
            DateTime currentDate = DateTime.Now.Date;

            // Calculate the number of days elapsed between the current date and the start date
            int daysElapsed = (currentDate - startDate).Days;

            // Calculate the current week based on the number of days elapsed
            int currentWeek = (daysElapsed / 7) + 1;
  
            // Check if the current week is within the provided number of weeks
            if (currentWeek <= numberOfWeeks && currentWeek >= 1)
            {
                return currentWeek;
            }
            else
            {
                // Handle the case where the current week exceeds the provided number of weeks
                return -1;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to get the week number of the year for a given date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetWeekOfYear(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday
            );
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// method to calculate the first date of the week for a given year and week number
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekNumber"></param>
        /// <returns></returns>
        private DateTime FirstDateOfWeek(int year, int weekNumber)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysToFirstDay = DayOfWeek.Monday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysToFirstDay);

            int daysToTargetWeek = 7 * (weekNumber - 1);
            DateTime firstDayOfWeek = firstMonday.AddDays(daysToTargetWeek);

            return firstDayOfWeek;
        }


    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------------------Eugene*End..
