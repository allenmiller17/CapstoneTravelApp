using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CapstoneTravelApp.DatabaseTables
{
    public class Admin_Table
    {
        [PrimaryKey, AutoIncrement]
        public int AdminId { get; set; }

        [NotNull]
        public string AdminUserName {get; set;}

        [NotNull]
        public string AdminPassword { get; set; }

        [NotNull]
        public string AdminEmail { get; set; }
    }
}
