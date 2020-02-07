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
	public partial class TripOverviewPage : ContentPage
	{
        private SQLiteConnection conn;
        private ObservableCollection<Flights_Table> thisTrip;
        private ObservableCollection<Lodging_Table> _lodgingList;
        private Trips_Table CurrentTrip;

		public TripOverviewPage (Trips_Table trip)
		{
			InitializeComponent ();

            CurrentTrip = trip;

            Title = $"{CurrentTrip.TripName}";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();


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

            DateTime flightTime = CurrentTrip.TripStart;

            var _flights = conn.Query<Flights_Table>($"SELECT * FROM Flights_Table WHERE TripId = '{CurrentTrip.TripId}'").Take(1);
            

            thisTrip = new ObservableCollection<Flights_Table>(_flights);
            FlightsListView.ItemsSource = thisTrip;

            var lodging = conn.Query<Lodging_Table>($"SELECT * FROM Lodging_Table WHERE TripId = '{CurrentTrip.TripId}'").Take(1);


            _lodgingList = new ObservableCollection<Lodging_Table>(lodging);
            LodgingListView.ItemsSource = _lodgingList;

            base.OnAppearing();
        }
    }
}