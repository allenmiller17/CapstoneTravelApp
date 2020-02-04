using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CapstoneTravelApp.DatabaseTables;
using Xamarin.Forms;
using SQLite;
using System.IO;
using CapstoneTravelApp.Droid;
using CapstoneTravelApp.Droid.DatabaseTables;

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