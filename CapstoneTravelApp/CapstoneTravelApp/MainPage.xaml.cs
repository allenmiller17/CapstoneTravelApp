using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.DatabaseTables;
using SQLite;
using CapstoneTravelApp.HelperFolders;
using System.Collections.ObjectModel;

namespace CapstoneTravelApp
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection conn;
        private ObservableCollection<User_Table> UserList;
        
        public MainPage()
        {
            InitializeComponent();

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            NavigationPage.SetHasNavigationBar(this.Main, true);

            conn.DropTable<User_Table>();
            conn.DropTable<Trips_Table>();

            conn.CreateTable<User_Table>();

            var _UserList = conn.Query<User_Table>($"SELECT * FROM User_Table");

            UsersListView.ItemsSource = _UserList;
            

            UsersListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(User_Clicked);
            if (!_UserList.Any())
            {
                var newUser = new User_Table();
                newUser.UserId = 10;
                newUser.UserName = "allen1";
                newUser.Password = "allen1";
                newUser.FirstName = "Allen";
                newUser.LastName = "Miller";
                newUser.UserEmail = "17allenmiller@gmail.com";

                conn.InsertOrReplace(newUser);
                _UserList.Add(newUser);

                var newUser1 = new User_Table();
                newUser1.UserId = 10;
                newUser1.UserName = "allen2";
                newUser1.Password = "allen2";
                newUser1.FirstName = "Allen2";
                newUser1.LastName = "Miller2";
                newUser1.UserEmail = "0217allenmiller@gmail.com";

                conn.InsertOrReplace(newUser1);
                _UserList.Add(newUser1);
            }

            UsersListView.ItemsSource = _UserList;
        }

        private async void User_Clicked(object sender, ItemTappedEventArgs e)
        {
            User_Table user = (User_Table)e.Item;
            await Navigation.PushAsync(new LoginPage(user));
        }
    }
}
