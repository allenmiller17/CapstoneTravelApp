using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.Droid.DatabaseTables;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(TravelApp_db))]
namespace CapstoneTravelApp.Droid.DatabaseTables
{
    public class TravelApp_db : ITravelApp_db
    {
        public SQLiteConnection GetConnection()
        {
            string dbName = "TravelApp_db.sqlite";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullPath = Path.Combine(folderPath, dbName);

            return new SQLiteConnection(fullPath);
        }
    }
}