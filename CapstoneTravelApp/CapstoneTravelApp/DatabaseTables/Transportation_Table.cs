using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Transportation_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int RentalId { get; set; }

        [NotNull]
        public string RentalName { get; set; }

        [NotNull]
        public string ConfNumber { get; set; }


        public DateTime PickUpDate { get; set; }


        public DateTime ReturnDate { get; set; }


        public string PickUpLocation { get; set; }


        public string ReturnLocation { get; set; }

        public string RentalPhone { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int RentalNotifications { get; set; }
    }
}
