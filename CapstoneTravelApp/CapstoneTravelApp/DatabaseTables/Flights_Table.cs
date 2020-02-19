using SQLite;
using System;
namespace CapstoneTravelApp.DatabaseTables
{
    public class Flights_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int FlightId { get; set; }

        [NotNull]
        public string FlightNumber { get; set; }

        [NotNull]
        public string AirlineName { get; set; }


        public DateTime DepartTime { get; set; }


        public DateTime ArriveTime { get; set; }


        public string DepartLocation { get; set; }


        public string ArriveLocation { get; set; }


        public string DepartGate { get; set; }

        [NotNull]
        public int TripId { get; set; }


        public int FlightNotifications { get; set; }

        public string FlightNotes { get; set; }
    }
}
