using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Lodging_Table
    {
        [PrimaryKey, AutoIncrement]
        public int LodgeId { get; set; }

        [NotNull]
        public string LodgeName { get; set; }


        public string LodgeLocation { get; set; }


        public DateTime LodgeStart { get; set; }


        public DateTime LodgeEnd { get; set; }

        [MaxLength(10)]
        public long LodgePhone { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int LodgeNotifications { get; set; }
    }
}
