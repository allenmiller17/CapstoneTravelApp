﻿using SQLite;


namespace CapstoneTravelApp.DatabaseTables
{
    public class User_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UserId { get; set; }

        [NotNull]
        [Unique]
        public string UserName { get; set; }

        [NotNull]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotNull]
        public string UserEmail { get; set; }

        public User_Table() { }
    }
}
