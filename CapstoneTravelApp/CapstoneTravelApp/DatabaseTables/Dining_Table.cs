using SQLite;
using System;
namespace CapstoneTravelApp.DatabaseTables
{
    public class Dining_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ResId { get; set; }

        [NotNull]
        public string ResName { get; set; }


        public string ResAddress { get; set; }


        public DateTime ResDate { get; set; }

        public string ResPhone { get; set; }


        public string ResNotes { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int ResNotifications { get; set; }
    }
}
