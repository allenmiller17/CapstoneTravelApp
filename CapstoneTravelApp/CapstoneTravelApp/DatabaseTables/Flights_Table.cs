using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace CapstoneTravelApp.DatabaseTables
{
    public class Flights_Table
    {
        [PrimaryKey, AutoIncrement]
        public int  FlightId { get; set; }

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
    }
}
