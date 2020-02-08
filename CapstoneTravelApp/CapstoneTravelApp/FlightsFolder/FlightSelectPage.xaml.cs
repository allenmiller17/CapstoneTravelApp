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
using CapstoneTravelApp.FlightsFolder;

namespace CapstoneTravelApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlightSelectPage : ContentPage
	{
        private SQLiteConnection conn;
        private Trips_Table _currentTrip;
        private ObservableCollection<Flights_Table> flightsList;

        public FlightSelectPage (Trips_Table CurrentTrip)
		{
			InitializeComponent ();

            _currentTrip = CurrentTrip;

            Title = $"{_currentTrip.TripName}";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            FlightsListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Flight_Tapped);
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Flights_Table>();
            var _flightsList = conn.Query<Flights_Table>($"SELECT * FROM Flights_Table WHERE TripId = '{_currentTrip.TripId}'");
            flightsList = new ObservableCollection<Flights_Table>(_flightsList);

            FlightsListView.ItemsSource = flightsList;


            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Menu", "Cancel", null, "Add Flight", "Share Flight List");
            if (action == "Add Flight")
            {
                await Navigation.PushModalAsync(new AddFlightPage(_currentTrip));
            }
            else if (action == "Share Flight List")
            {
                //Add Sharing Controls
            }
        }

        private async void Flight_Tapped(object sender, ItemTappedEventArgs e)
        {
            Flights_Table currentFlight = (Flights_Table)e.Item;
            await Navigation.PushAsync(new FlightInfoPage(currentFlight));
        }
    }
}