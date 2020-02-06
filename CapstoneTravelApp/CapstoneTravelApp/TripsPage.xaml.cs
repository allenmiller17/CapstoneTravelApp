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
        private Trips_Table CurrentUser;

        public TripsPage(Trips_Table currentUser)
        {
            InitializeComponent();

            CurrentUser = currentUser;

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

            if (!trips.Any())
            {
                #region Trips Data
                var newTrip = new Trips_Table();
                newTrip.UserName = "allen1";
                newTrip.TripName = "DisneyLand 2020";
                newTrip.TripStart = new DateTime(2020, 11, 12);
                newTrip.TripEnd = new DateTime(2020, 11, 17);
                newTrip.Notes = "We're In Disneyland Evan";
                newTrip.TripNotifications = 1;

                conn.InsertOrReplace(newTrip);
                trips.Add(newTrip);

                var newTrip2 = new Trips_Table();
                newTrip2.UserName = "allen2";
                newTrip2.TripName = "Florida 2021";
                newTrip2.TripStart = new DateTime(2021, 11, 12);
                newTrip2.TripEnd = new DateTime(2021, 11, 17);
                newTrip2.Notes = "We're In Florida Evan";
                newTrip2.TripNotifications = 0;

                conn.InsertOrReplace(newTrip2);
                trips.Add(newTrip2);

                var newTrip3 = new Trips_Table();
                newTrip3.UserName = "allen1";
                newTrip3.TripName = "DisneyLand 2020";
                newTrip3.TripStart = new DateTime(2020, 11, 12);
                newTrip3.TripEnd = new DateTime(2020, 11, 17);
                newTrip3.Notes = "We're In Disneyland Evan";
                newTrip3.TripNotifications = 1;

                conn.InsertOrReplace(newTrip3);
                trips.Add(newTrip3);

                var newTrip4 = new Trips_Table();
                newTrip4.UserName = "allen2";
                newTrip4.TripName = "Florida 2021";
                newTrip4.TripStart = new DateTime(2021, 11, 12);
                newTrip4.TripEnd = new DateTime(2021, 11, 17);
                newTrip4.Notes = "We're In Florida Evan";
                newTrip4.TripNotifications = 0;

                conn.InsertOrReplace(newTrip4);
                trips.Add(newTrip4);
                #endregion

                #region Dining Data
                var newRes = new Dining_Table();
                newRes.ResName = "Carnation Cafe";
                newRes.ResAddress = "Disneyland Park, Anaheim, CA 92802";
                newRes.ResPhone = 7147813463;
                newRes.ResNotes = "Dinner with my wife";
                newRes.ResNotifications = 1;
                newRes.TripId = newTrip.TripId;

                conn.Insert(newRes);

                var newRes2 = new Dining_Table();
                newRes2.ResName = "Rainforest Cafe";
                newRes2.ResAddress = "123 Downtown Disney District, Anaheim, CA 92802";
                newRes2.ResPhone = 7148665555;
                newRes2.ResNotes = "Birthday Dinner";
                newRes2.ResNotifications = 1;
                newRes2.TripId = newTrip.TripId;

                conn.Insert(newRes2);
                #endregion

                #region Entertainment Data
                var newActivity = new Entertainment_Table();
                newActivity.EntertainName = "Disneyland";
                newActivity.EntertaninStart = new DateTime(2020, 11, 12, 08, 00, 00);
                newActivity.EntertainEnd = new DateTime(2020, 11, 12, 23, 00, 00);
                newActivity.EnterainAddress = "1313 Disneyland Dr, Anaheim, CA 92802";
                newActivity.EntertainPhone = 7147814636;
                newActivity.EntertainNotes = "Get to the park at 7:00";
                newActivity.EntertainNotifications = 1;
                newActivity.TripId = newTrip.TripId;

                conn.Insert(newActivity);

                var newActivity2 = new Entertainment_Table();
                newActivity2.EntertainName = "Disneyland";
                newActivity2.EntertaninStart = new DateTime(2020, 11, 14, 07, 00, 00);
                newActivity2.EntertainEnd = new DateTime(2020, 11, 15, 00, 00, 00);
                newActivity2.EnterainAddress = "1313 Disneyland Dr, Anaheim, CA 92802";
                newActivity2.EntertainPhone = 7147814636;
                newActivity2.EntertainNotes = "Get to the park at 6:00";
                newActivity2.EntertainNotifications = 1;
                newActivity2.TripId = newTrip.TripId;

                conn.Insert(newActivity2);
                #endregion

                #region Flights Data
                var newflight = new Flights_Table();
                //Add Flight Data Here
                conn.Insert(newflight);

                #endregion

            }


            TripsList = new ObservableCollection<Trips_Table>(trips);
            
            TripsListView.ItemsSource = TripsList;

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