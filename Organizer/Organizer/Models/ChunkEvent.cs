using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Organizer.Models
{
    public class ChunkEvent
    {
        [ForeignKey(typeof(Chunk))]
        public int ChunkID { get; set; }
        [ForeignKey(typeof(Event))]
        public int EventID { get; set; }
    }
}
