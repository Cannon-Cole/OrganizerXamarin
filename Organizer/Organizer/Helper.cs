using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Organizer
{
    class Helper
    {
        public static Boolean toDoNeedsLoading;
        public static Boolean monthNeedsLoading;
        public static Boolean addEventChunkListNeedsLoading;
        public static double scrollPosition;
        public static Color notComplete = Color.Coral;
        public static Color complete = Color.Beige;
        public static Color eventChunkSelectedColor = Color.BurlyWood;

        public static void OutputEventToConsole(Organizer.Models.Event showEvent)
        {
            Console.WriteLine("Event ID: " + showEvent.EventID);
            Console.WriteLine("Event Name: " + showEvent.Name);
            Console.WriteLine("Event Note: " + showEvent.Note);
            Console.WriteLine("Event Start Date: " + showEvent.StartDate);
            Console.WriteLine("Event End Date: " + showEvent.EndDate);
            Console.WriteLine("Event Start Time: " + showEvent.StartTime);
            Console.WriteLine("Event End Time: " + showEvent.EndTime);
            Console.WriteLine("Event Complete: " + showEvent.Complete);

        }

        public static string AddZeroToSingleDigit(int number)
        {
            if (number < 10)
            {
                return "0" + number;
            }
            else
            {
                return "" + number;
            }
        }

        public static string prepareDateForDB(DateTime dateToFormat)
        {
            string singleDigitDay = Helper.AddZeroToSingleDigit(dateToFormat.Date.Day);

            string singleDigitMonth = Helper.AddZeroToSingleDigit(dateToFormat.Date.Month);

            return "\"" + dateToFormat.Year + "-" + singleDigitMonth + "-" + singleDigitDay + "T00:00:00.000" + "\"";
        }

        public static string TimeSpanToAMPM(TimeSpan timeToFormat)
        {
            string formattedTime = "";

            if(timeToFormat.Hours == 0)
            {
                return 12 + ":" + AddZeroToSingleDigit(timeToFormat.Minutes) + " AM";
            }
            else if (timeToFormat.Hours <= 12)
            {
                return AddZeroToSingleDigit(timeToFormat.Hours) + ":" + AddZeroToSingleDigit(timeToFormat.Minutes) + " AM";
            }
            else
            {
                return formattedTime = AddZeroToSingleDigit(timeToFormat.Hours - 12) + ":" + AddZeroToSingleDigit(timeToFormat.Minutes) + " PM";
            }
        }
    }
}
