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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripsPage : ContentPage
    {
        UserHelper userHelper = new UserHelper();
        private SQLiteConnection conn;
        private ObservableCollection<Trips_Table> TripsList;
        private string CurrentUser;

        public TripsPage(string _userName)
        {
            InitializeComponent();

            CurrentUser = _userName;

            Title = "My Trips";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();


            TripsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Trip_Tapped);

            NavigationPage.SetHasBackButton(this, true);
        }

        protected override void OnAppearing()
        {

            conn.CreateTable<Trips_Table>();
            conn.CreateTable<Flights_Table>();
            conn.CreateTable<Dining_Table>();
            conn.CreateTable<Entertainment_Table>();
            conn.CreateTable<Lodging_Table>();
            conn.CreateTable<Transportation_Table>();
            conn.CreateTable<User_Table>();

            var trips = conn.Query<Trips_Table>($"SELECT * FROM Trips_Table WHERE UserName = '{CurrentUser}'");

            TripsList = new ObservableCollection<Trips_Table>(trips);
            
            TripsListView.ItemsSource = TripsList.OrderBy(d => d.TripStart);

            base.OnAppearing();
        }

        private void AddTripButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void Trip_Tapped(object sender, ItemTappedEventArgs e)
        {
            Trips_Table trip = (Trips_Table)e.Item;
            await Navigation.PushAsync(new TripOverviewPage(trip));
        }
    }
}