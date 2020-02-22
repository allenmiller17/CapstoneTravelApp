using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using CapstoneTravelApp.TripsFolder;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripsPage : ContentPage
    {
        UserHelper userHelper = new UserHelper();
        private SQLiteConnection conn;
        private ObservableCollection<Trips_Table> TripsList;
        private string CurrentUser;
        private bool firstTime = true;
        private ObservableCollection<Dining_Table> diningList;
        private ObservableCollection<Entertainment_Table> activityList;
        private ObservableCollection<Flights_Table> flightsList;
        private ObservableCollection<Lodging_Table> lodgingList;
        public ObservableCollection<Transportation_Table> rentalList;

        public TripsPage(string _userName)
        {
            InitializeComponent();

            CurrentUser = _userName;

            Title = "My Trips";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();


            TripsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Trip_Tapped);

            NavigationPage.SetHasBackButton(this, true);
        }

        protected async override void OnAppearing()
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

            if (firstTime)
            {
                firstTime = false;


                #region Trips Notifications
                int tripInt = 0;
                foreach (Trips_Table trips_ in TripsList)
                {
                    tripInt++;
                    if (trips_.TripNotifications == 1)
                    {
                        if (trips_.TripStart.AddDays(-1) == DateTime.Today)
                        {
                            CrossLocalNotifications.Current.Show("Reminder", $"{trips_.TripName} begins tomorrow.", tripInt);
                            await DisplayAlert("Notice", trips_.TripName + " is starting tomorrow", "Ok");
                        }
                    }
                }
                #endregion
                int tripId = 0;
                foreach (Trips_Table currentTrip in TripsList)
                {
                    tripId++;
                    #region Dining Notification
                    var _diningList = conn.Query<Dining_Table>($"SELECT * FROM Dining_Table WHERE TripId = '{currentTrip.TripId}'");
                    diningList = new ObservableCollection<Dining_Table>(_diningList);
                    foreach (Dining_Table dining in diningList)
                    {
                        if (dining.ResNotifications == 1)
                        {
                            if (dining.ResDate.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{dining.ResName} begins tomorrow.", tripId);
                                await DisplayAlert("Notice", dining.ResName + " is tomorrow", "Ok");
                            }
                        }
                    }
                    #endregion

                    #region Entertainment Notifications
                    var _activityList = conn.Query<Entertainment_Table>($"SELECT * FROM Entertainment_Table WHERE TripId = '{currentTrip.TripId}'");
                    activityList = new ObservableCollection<Entertainment_Table>(_activityList);
                    foreach (Entertainment_Table activity in activityList)
                    {
                        if (activity.EntertainNotifications == 1)
                        {
                            if (activity.EntertaninStart.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{activity.EntertainName} begins tomorrow.", tripId);
                                await DisplayAlert("Notice", activity.EntertainName + " is tomorrow", "Ok");
                            }
                        }
                    }
                    #endregion

                    #region Flight Notifications
                    var _flightsList = conn.Query<Flights_Table>($"SELECT * FROM Flights_Table WHERE TripId = '{currentTrip.TripId}'");
                    flightsList = new ObservableCollection<Flights_Table>(_flightsList);
                    foreach (Flights_Table flight in flightsList)
                    {
                        if (flight.FlightNotifications == 1)
                        {
                            if (flight.DepartTime.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{flight.AirlineName} leaves tomorrow.", tripId);
                                await DisplayAlert("Notice", flight.AirlineName + " " + flight.FlightNumber + " leaves tomorrow", "Ok");
                            }
                        }
                    }
                    #endregion

                    #region Lodging Notifications
                    var _lodgingList = conn.Query<Lodging_Table>($"SELECT * FROM Lodging_Table WHERE TripId = '{currentTrip.TripId}'");
                    lodgingList = new ObservableCollection<Lodging_Table>(_lodgingList);
                    foreach (Lodging_Table hotel in lodgingList)
                    {
                        if (hotel.LodgeNotifications == 1)
                        {
                            if (hotel.LodgeStart.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{hotel.LodgeName} stay is tomorrow.", tripId);
                                await DisplayAlert("Notice", hotel.LodgeName + " stay is tomorrow", "Ok");
                            }
                        }
                    }
                    #endregion

                    #region Transportation Notifications
                    var _rentalList = conn.Query<Transportation_Table>($"SELECT * FROM Transportation_Table WHERE TripId = '{currentTrip.TripId}'");
                    rentalList = new ObservableCollection<Transportation_Table>(_rentalList);
                    foreach (Transportation_Table rental in rentalList)
                    {
                        if (rental.RentalNotifications == 1)
                        {
                            if (rental.PickUpDate.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{rental.RentalName} reservation is tomorrow.", tripId);
                                await DisplayAlert("Notice", rental.RentalName + " reservation is tomorrow", "Ok");
                            }

                            if (rental.ReturnDate.AddDays(-1).ToShortDateString() == DateTime.Today.ToShortDateString())
                            {
                                CrossLocalNotifications.Current.Show("Reminder", $"{rental.RentalName} drop off is tomorrow.", tripId);
                                await DisplayAlert("Notice", rental.RentalName + " drop off is tomorrow", "Ok");
                            }
                        }
                    }
                    #endregion
                }
            }
            base.OnAppearing();
        }

        private void AddTripButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTripPage(CurrentUser));
        }

        private async void Trip_Tapped(object sender, ItemTappedEventArgs e)
        {
            Trips_Table trip = (Trips_Table)e.Item;
            await Navigation.PushAsync(new TripOverviewPage(trip));
        }
    }
}