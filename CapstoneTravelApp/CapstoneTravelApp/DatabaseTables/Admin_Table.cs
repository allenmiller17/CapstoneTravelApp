﻿using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Admin_Table
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int AdminId { get; set; }

        [NotNull]
        public string AdminUserName { get; set; }

        [NotNull]
        public string AdminPassword { get; set; }

        [NotNull]
        public string AdminEmail { get; set; }
    }
}
