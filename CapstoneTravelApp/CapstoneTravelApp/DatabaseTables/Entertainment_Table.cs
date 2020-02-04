using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Entertainment_Table
    {
        [PrimaryKey, AutoIncrement]
        public int EntertainId { get; set; }

        [NotNull]
        public string EntertainName { get; set; }


        public DateTime EntertaninStart { get; set; }


        public DateTime EntertainEnd { get; set; }


        public string EnterainAddress { get; set; }

        [MaxLength(10)]
        public int EntertainPhone { get; set; }


        public string EntertainNotes { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int EntertainNotifications { get; set; }
    }
}
