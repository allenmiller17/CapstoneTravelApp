using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Trips_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int  TripId { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        [NotNull]
        public string TripName { get; set; }


        public DateTime TripStart { get; set; }


        public DateTime TripEnd { get; set; }


        public string Notes { get; set; }


        public int TripNotifications { get; set; }
    }
}
