using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Organizer.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int EventID { get; set; }
        public String Name { get; set; }
        public String Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Complete { get; set; }
    }
}