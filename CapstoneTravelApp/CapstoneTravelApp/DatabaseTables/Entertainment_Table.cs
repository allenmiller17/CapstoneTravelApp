using SQLite;
using System;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Entertainment_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int EntertainId { get; set; }

        [NotNull]
        public string EntertainName { get; set; }


        public DateTime EntertaninStart { get; set; }


        public DateTime EntertainEnd { get; set; }


        public string EnterainAddress { get; set; }

        public string EntertainPhone { get; set; }


        public string EntertainNotes { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int EntertainNotifications { get; set; }
    }
}
