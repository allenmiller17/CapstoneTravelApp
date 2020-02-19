using SQLite;


namespace CapstoneTravelApp.DatabaseTables
{
    public interface ITravelApp_db
    {
        SQLiteConnection GetConnection();
    }
}
