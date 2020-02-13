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

            Title = "Welcome";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            NavigationPage.SetHasNavigationBar(this.Main, true);

            conn.DropTable<User_Table>();
            conn.DropTable<Trips_Table>();
            conn.DropTable<Flights_Table>();
            conn.DropTable<Dining_Table>();
            conn.DropTable<Entertainment_Table>();
            conn.DropTable<Lodging_Table>();
            conn.DropTable<Transportation_Table>();

            conn.CreateTable<User_Table>();
            conn.CreateTable<Trips_Table>();
            conn.CreateTable<Flights_Table>();
            conn.CreateTable<Dining_Table>();
            conn.CreateTable<Entertainment_Table>();
            conn.CreateTable<Lodging_Table>();
            conn.CreateTable<Transportation_Table>();

            var _UserList = conn.Query<User_Table>($"SELECT * FROM User_Table");

            if (!_UserList.Any())
            {



                #region User Data
                var newUser = new User_Table();
                newUser.UserName = "allen1";
                newUser.Password = "allen1";
                newUser.FirstName = "Allen";
                newUser.LastName = "Miller";
                newUser.UserEmail = "17allenmiller@gmail.com";

                conn.Insert(newUser);
                _UserList.Add(newUser);

                var newUser1 = new User_Table();
                newUser1.UserName = "allen2";
                newUser1.Password = "allen2";
                newUser1.FirstName = "Allen2";
                newUser1.LastName = "Miller2";
                newUser1.UserEmail = "0217allenmiller@gmail.com";

                conn.Insert(newUser1);
                _UserList.Add(newUser1);

                #endregion

                #region Trips Data

                var newTrip2 = new Trips_Table();
                newTrip2.UserName = newUser.UserName;
                newTrip2.TripName = "Florida 2021";
                newTrip2.TripStart = new DateTime(2021, 11, 12);
                newTrip2.TripEnd = new DateTime(2021, 11, 17);
                newTrip2.Notes = "We're In Florida Evan";
                newTrip2.TripNotifications = 0;
                newTrip2.UserId = newUser.UserId;

                conn.Insert(newTrip2);

                var newTrip3 = new Trips_Table();
                newTrip3.UserName = newUser1.UserName;
                newTrip3.TripName = "DisneyLand 2020";
                newTrip3.TripStart = new DateTime(2020, 11, 12);
                newTrip3.TripEnd = new DateTime(2020, 11, 17);
                newTrip3.Notes = "We're In Disneyland Evan";
                newTrip3.TripNotifications = 1;
                newTrip3.UserId = newUser1.UserId;

                conn.Insert(newTrip3);

                #endregion

                #region Dining Data
                var newRes = new Dining_Table();
                newRes.ResName = "Carnation Cafe";
                newRes.ResAddress = "Disneyland Park, Anaheim, CA 92802";
                newRes.ResPhone = 7147813463;
                newRes.ResNotes = "Dinner with my wife";
                newRes.ResNotifications = 1;
                newRes.TripId = newTrip2.TripId;

                conn.Insert(newRes);

                var newRes2 = new Dining_Table();
                newRes2.ResName = "Rainforest Cafe";
                newRes2.ResAddress = "123 Downtown Disney District, Anaheim, CA 92802";
                newRes2.ResPhone = 7148665555;
                newRes2.ResNotes = "Birthday Dinner";
                newRes2.ResNotifications = 1;
                newRes2.TripId = newTrip2.TripId;

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
                newActivity.TripId = newTrip2.TripId;

                conn.Insert(newActivity);

                var newActivity2 = new Entertainment_Table();
                newActivity2.EntertainName = "Disneyland";
                newActivity2.EntertaninStart = new DateTime(2020, 11, 14, 07, 00, 00);
                newActivity2.EntertainEnd = new DateTime(2020, 11, 15, 00, 00, 00);
                newActivity2.EnterainAddress = "1313 Disneyland Dr, Anaheim, CA 92802";
                newActivity2.EntertainPhone = 7147814636;
                newActivity2.EntertainNotes = "Get to the park at 6:00";
                newActivity2.EntertainNotifications = 1;
                newActivity2.TripId = newTrip2.TripId;

                conn.Insert(newActivity2);
                #endregion

                #region Flights Data
                var newflight = new Flights_Table();
                newflight.FlightNumber = "SWA123";
                newflight.AirlineName = "Southwest";
                newflight.DepartTime = new DateTime(2020, 11, 12, 1, 45, 00);
                newflight.ArriveTime = new DateTime(2020, 11, 12, 3, 30, 00);
                newflight.DepartLocation = "SLC";
                newflight.ArriveLocation = "LAX";
                newflight.DepartGate = "A5";
                newflight.FlightNotifications = 1;
                newflight.TripId = newTrip2.TripId;

                conn.Insert(newflight);

                var newflight1 = new Flights_Table();
                newflight1.FlightNumber = "SWA987";
                newflight1.AirlineName = "Southwest";
                newflight1.DepartTime = new DateTime(2020, 11, 17, 8, 00, 00);
                newflight1.ArriveTime = new DateTime(2020, 11, 17, 10, 10, 00);
                newflight1.DepartLocation = "LAX";
                newflight1.ArriveLocation = "SLC";
                newflight1.DepartGate = "G13";
                newflight1.FlightNotifications = 0;
                newflight1.TripId = newTrip2.TripId;

                conn.Insert(newflight1);

                #endregion

                #region Lodging Data
                var newLodge = new Lodging_Table();
                newLodge.LodgeName = "Best Western Plus Pavilions";
                newLodge.LodgeLocation = "1176 W Katella Ave, Anaheim, CA 92802";
                newLodge.LodgePhone = "7147760140";
                newLodge.LodgeStart = new DateTime(2020, 11, 12, 11, 00, 00);
                newLodge.LodgeEnd = new DateTime(2020, 11, 15, 9, 00, 00);
                newLodge.LodgeNotifications = 1;
                newLodge.TripId = newTrip2.TripId;

                conn.Insert(newLodge);

                var newLodge1 = new Lodging_Table();
                newLodge1.LodgeName = "Best Western Plus Stovals";
                newLodge1.LodgeLocation = "1100 W Katella Ave, Anaheim, CA 92802";
                newLodge1.LodgePhone = "7147765555";
                newLodge1.LodgeStart = new DateTime(2020, 11, 15, 11, 00, 00);
                newLodge1.LodgeEnd = new DateTime(2020, 11, 17, 9, 00, 00);
                newLodge1.LodgeNotifications = 0;
                newLodge1.TripId = newTrip2.TripId;

                conn.Insert(newLodge1);

                #endregion

                #region Transportation Data
                var newRental = new Transportation_Table();
                newRental.RentalName = "Hertz";
                newRental.ConfNumber = "1E0C2G";
                newRental.PickUpDate = new DateTime(2020, 11, 12);
                newRental.ReturnDate = new DateTime(2020, 11, 17);
                newRental.PickUpLocation = "LAX";
                newRental.ReturnLocation = "LAX";
                newRental.RentalPhone = 5558885555;
                newRental.RentalNotifications = 1;
                newRental.TripId = newTrip2.TripId;

                conn.Insert(newRental);

                #endregion
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterationPage());
        }
    }
}
