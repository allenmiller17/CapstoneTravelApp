using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using CapstoneTravelApp.DatabaseTables;
using System;

namespace CapstoneTravelApp.HelperFolders
{
    public class UserHelper
    {
        private SQLiteConnection _SQLiteConnection;

        public UserHelper()
        {
            _SQLiteConnection = DependencyService.Get<ITravelApp_db>().GetConnection();
            _SQLiteConnection.CreateTable<User_Table>();
            _SQLiteConnection.CreateTable<Admin_Table>();
        }

        public IEnumerable<User_Table> GetUsers()
        {
            return (from u in _SQLiteConnection.Table<User_Table>() select u).ToList();
        }

        public IEnumerable<Admin_Table> GetAdmins()
        {
            return (from a in _SQLiteConnection.Table<Admin_Table>() select a).ToList();
        }

        //public User_Table GetSpecificUser(int id)
        //{
        //    return _SQLiteConnection.Table<User_Table>().FirstOrDefault(t => t.UserId == id);
        //}

        public Trips_Table GetSpecificUsername(string userName)
        {
            return _SQLiteConnection.Table<Trips_Table>().FirstOrDefault(un => un.UserName == userName);
        }

        public void DeleteUser (int id)
        {
            _SQLiteConnection.Delete<User_Table>(id);
        }

        public string AddUser(User_Table user)
        {
            var data = _SQLiteConnection.Table<User_Table>();
            var d1 = data.Where(f => f.FirstName == user.FirstName && f.LastName == user.LastName && f.UserName == user.UserName)
                .FirstOrDefault(f => f.UserId == user.UserId);

            if (d1 == null)
            {
                _SQLiteConnection.Insert(user);
                return "User Added";
            }
            else
            {
                return "A user with this name already exists!";
            }
        }

        public bool UpdateUser(string username, string pass)
        {
            var data = _SQLiteConnection.Table<User_Table>();
            var d1 = (from values in data where values.UserName == username select values).Single();

            if (true)
            {
                d1.Password = pass;
                _SQLiteConnection.Update(d1);
                return true;
            }
        }

        public bool LoginValidate(string userName1, string pass1)
        {
            var data = _SQLiteConnection.Table<User_Table>();
            var d1 = data.Where(f => f.UserName == userName1 && f.Password == pass1).FirstOrDefault();

            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }

        public bool AdminValidate(string userName2, string pass2)
        {
            var data = _SQLiteConnection.Table<Admin_Table>();
            var d1 = data.Where(a => a.AdminUserName == userName2 && a.AdminPassword == pass2).FirstOrDefault();

            if (d1 != null)
            {
                return true;
            }
            else
                return false;
        }

        public bool ValidPhoneNumber (string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }


            //Exception Handeling
            //Checks to see if there are extra characters in the number and returns an error message if there are
            try
            {
                var IntPhone = Convert.ToInt64(phone);
            }
            catch (FormatException)
            {

                return false;
            }

            return true;
        }
    }
}
