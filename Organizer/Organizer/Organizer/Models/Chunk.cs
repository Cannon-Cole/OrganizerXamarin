using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Organizer.Models
{
    public class Chunk
    {
        [PrimaryKey, AutoIncrement]
        public int ChunkID { get; set; }
        public String Name { get; set; }
        public String Note { get; set; }
        public String Color { get; set; }
        public String Repeats { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
