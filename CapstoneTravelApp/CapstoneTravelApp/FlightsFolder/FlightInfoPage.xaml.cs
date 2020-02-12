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
using Xamarin.Essentials;

namespace CapstoneTravelApp.FlightsFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlightInfoPage : ContentPage
	{
        private Flights_Table _currentFlight;
        private SQLiteConnection conn;
        private ObservableCollection<Flights_Table> _flightsList;

		public FlightInfoPage (Flights_Table currentFlight)
		{
			InitializeComponent ();

            _currentFlight = currentFlight;

            Title = _currentFlight.AirlineName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

        protected override void OnAppearing()
        {
            conn.CreateTable<Flights_Table>();
            var flightsList = conn.Query<Flights_Table>($"SELECT * FROM Flights_Table WHERE FlightID = '{_currentFlight.FlightId}'");
            _flightsList = new ObservableCollection<Flights_Table>(flightsList);

            flighNameLabel.Text = _currentFlight.AirlineName;
            flightNumberLabel.Text = "Flight: " + $"{_currentFlight.FlightNumber}";
            departGatelabel.Text = "Departure Gate: " + $"{_currentFlight.DepartGate}";
            departLocLabel.Text = "Departs: " + $"{_currentFlight.DepartLocation}";
            departTimeLabel.Text = $"{_currentFlight.DepartTime.ToString("MM/dd HH:mm tt")}";
            arriveLocLabel.Text = "Arrives: " + $"{_currentFlight.ArriveLocation}";
            arriveTimeLabel.Text = $"{_currentFlight.ArriveTime.ToString("MM/dd HH:mm tt")}";
            notificationSwitch.IsToggled = _currentFlight.FlightNotifications == 1 ? true : false;



            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Flight Options", "Cancel", "Delete Flight", "Edit Flight", "Share Flight");
            if (action == "Edit Flight")
            {
                await Navigation.PushModalAsync(new EditFlightPage(_currentFlight));
            }
            else if (action == "Share Flight")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = flighNameLabel.Text + "\n" + flightNumberLabel.Text + "\n" + departGatelabel.Text +
                    "\n" + departLocLabel.Text + "\n" + departTimeLabel.Text + "\n" + arriveLocLabel.Text + "\n" + arriveTimeLabel.Text + "\n"
                    + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
            else if (action == "Delete Flight")
            {
                var deleteResponse = await DisplayAlert("Warning", "You are about to delete this flight! Are you sure?", "Yes", "No");
                if (deleteResponse)
                {
                    conn.Delete(_currentFlight);
                    await Navigation.PopAsync();
                }
            }
        }

        private async void FlightNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FlightNotesPage(_currentFlight));
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            _currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentFlight);
        }
    }
}