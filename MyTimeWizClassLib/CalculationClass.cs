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
                    studyHours = (Credits * 10 / NumberOfWeeks) - ClassHoursPerWeek;
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
        public int CalculateRemainingHoursForCurrentWeek(ModuleClass module)
        {
            DateTime currentDate = DateTime.Now.Date;
            int currentWeek = GetWeekOfYear(currentDate);

            // Calculate the start and end dates of the current week
            DateTime startOfWeek = FirstDateOfWeek(currentDate.Year, currentWeek);

            // my study week is only 6 days long and excludes Sundays
            DateTime endOfWeek = startOfWeek.AddDays(5);

            int totalStudiedHoursThisWeek = 0;

            foreach (var entry in module.StudiedHoursPerDate)
            {
                if (entry.Key >= startOfWeek && entry.Key <= endOfWeek)
                {
                    totalStudiedHoursThisWeek += entry.Value;
                }
            }

            int remainingHours = module.SelfStudyHours - totalStudiedHoursThisWeek;
            return remainingHours;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// ProgressBar calculation that will give percentage
        /// </summary>
        /// <returns></returns>
        public double ProgressBarCal(ModuleClass module)
        {
           return (module.RemainingWeekHours / Convert.ToDouble(module.SelfStudyHours)) * 100;

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
