using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace CapstoneTravelApp.DatabaseTables
{
    public interface ITravelApp_db
    {
        SQLiteConnection GetConnection();
    }
}
