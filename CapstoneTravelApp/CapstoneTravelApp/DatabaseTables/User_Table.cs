using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace CapstoneTravelApp.DatabaseTables
{
    public class User_Table
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }

        [NotNull]
        public string UserName { get; set; }

        [NotNull]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotNull]
        public string UserEmail { get; set; }

        public User_Table () {}
    }
}
