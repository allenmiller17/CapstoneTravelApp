//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using SQLite;
//using CapstoneTravelApp.DatabaseTables;
//using Xamarin.Forms;

//namespace CapstoneTravelApp.HelperFolders
//{
//    class Dictionary
//    {
//        private static Dictionary<int, Hashtable> _trips = new Dictionary<int, Hashtable>();
//        private static int UserId;
//        private static string userName;

//        private SQLiteConnection conn;

//        public static int getUserId()
//        {
//            return UserId;
//        }

//        public static void setUserId(int currentUserId)
//        {
//            UserId = currentUserId;
//        }

//        public static string getUserName()
//        {
//            return userName;
//        }

//        public static void setCurrentUserName(string currentUserName)
//        {
//            userName = currentUserName;
//        }

//        public static Dictionary<int, Hashtable> getTrips()
//        {
//            return _trips;
//        }

//        public static void setTrips(Dictionary<int, Hashtable> trips)
//        {
//            _trips = trips;
//        }

//        static public int findUser(string search)
//        {
//            int UserId;
//            string query;
//            if (int.TryParse(search, out UserId))
//            {
//                query = $"SELECT UserId FROM User_Table WHERE UserId = '{search.ToString()}'";
//            }
//            else
//            {
//                query = $"SELECT UserId FROM User_Table WHERE UserName LIKE '{search}'";
//            }
//            SQLiteConnection conn = DependencyService.Get<ITravelApp_db>().GetConnection();
//            SQLiteCommand cmd = new SQLiteCommand(query);
//        }
//    }
//}
