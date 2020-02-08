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

namespace CapstoneTravelApp.FlightsFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlightInfoPage : ContentPage
	{
        private Flights_Table _currentFlight;
        private SQLiteConnection conn;

		public FlightInfoPage (Flights_Table currentFlight)
		{
			InitializeComponent ();

            _currentFlight = currentFlight;

            Title = _currentFlight.AirlineName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

        protected override void OnAppearing()
        {
            flighNameLabel.Text = _currentFlight.AirlineName;
            flightNumberLabel.Text = "Flight: " + $"{_currentFlight.FlightNumber}";
            departGatelabel.Text = "Departure Gate: " + $"{_currentFlight.DepartGate}";
            departLocLabel.Text = "Departs: " + $"{_currentFlight.DepartLocation}";
            departTimeLabel.Text = _currentFlight.DepartTime.ToString("MM/dd HH:mm tt");
            arriveLocLabel.Text = "Arrives: " + $"{_currentFlight.ArriveLocation}";
            arriveTimeLabel.Text = _currentFlight.ArriveTime.ToString("MM/dd HH:mm tt");
            notificationSwitch.IsToggled = _currentFlight.FlightNotifications == 1 ? true : false;

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Menu", "Cancel", "Delete Flight", "Edit Flight", "Share Flight");
            if (action == "Edit Flight")
            {
                await Navigation.PushModalAsync(new EditFlightPage(_currentFlight));
            }
            else if (action == "Share Flight")
            {
                //Add Sharing Controls
            }
            else if (action == "Delete Flight")
            {
                var deleteResponse = await DisplayAlert("Warning", "Your are about to delete this flight! Are you sure?", "Yes", "No");
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

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            _currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentFlight);
        }
    }
}