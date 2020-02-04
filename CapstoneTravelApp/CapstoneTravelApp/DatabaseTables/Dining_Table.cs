using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace CapstoneTravelApp.DatabaseTables
{
    public class Dining_Table
    {
        [PrimaryKey, AutoIncrement]
        public int ResId { get; set; }

        [NotNull]
        public string ResName { get; set; }


        public string ResAddress { get; set; }


        public DateTime ResDate { get; set; }

        [MaxLength(10)]
        public int ResPhone { get; set; }

        
        public string ResNotes { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int ResNotifications { get; set; }
    }
}
