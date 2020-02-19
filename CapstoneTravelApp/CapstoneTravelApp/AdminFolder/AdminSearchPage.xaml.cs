using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using CapstoneTravelApp.TripsFolder;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.AdminFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminSearchPage : ContentPage
    {
        private SQLiteConnection conn;
        private ObservableCollection<User_Table> UserList;

        public AdminSearchPage()
        {
            InitializeComponent();

            Title = "User Search";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            UserListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(User_Tapped);
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<User_Table>();

            var User = conn.Query<User_Table>($"SELECT * FROM User_Table");
            UserList = new ObservableCollection<User_Table>(User);

            UserListView.ItemsSource = UserList.OrderBy(n => n.LastName);


            base.OnAppearing();
        }

        private async void AddAdminButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAdminPage());
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                UserListView.ItemsSource = UserList.OrderBy(n => n.LastName);
            }
            else
            {
                UserListView.ItemsSource = UserList.Where(l => l.LastName.StartsWith(e.NewTextValue)).OrderBy(n => n.LastName);
            }
        }

        private async void User_Tapped(object sender, ItemTappedEventArgs e)
        {
            User_Table user_ = (User_Table)e.Item;
            string userName = user_.UserName;
            await Navigation.PushAsync(new TripsPage(userName));
        }
    }
}