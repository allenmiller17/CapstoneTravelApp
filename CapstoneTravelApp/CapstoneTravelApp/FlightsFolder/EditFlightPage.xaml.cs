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
using System.Globalization;

namespace CapstoneTravelApp.FlightsFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditFlightPage : ContentPage
	{
        private Flights_Table currentFlight;
        private SQLiteConnection conn;

        public EditFlightPage (Flights_Table _currentFlight)
		{
			InitializeComponent ();
            currentFlight = _currentFlight;
            Title = currentFlight.AirlineName + "  " + currentFlight.FlightNumber;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

        protected override void OnAppearing()
        {
            conn.CreateTable<Flights_Table>();
            airlineNameEntry.Text = currentFlight.AirlineName;
            flightNumberEntry.Text = currentFlight.FlightNumber;
            departGateEntry.Text = currentFlight.DepartGate;
            departLocEntry.Text = currentFlight.DepartLocation;
            departDatePicker.Date = currentFlight.DepartTime;
            departTimePicker.Time = currentFlight.DepartTime.TimeOfDay;
            arriveLocEntry.Text = currentFlight.ArriveLocation;
            arriveDatePicker.Date = currentFlight.ArriveTime;
            arriveTimePicker.Time = currentFlight.ArriveTime.TimeOfDay;
            notificationSwitch.IsToggled = currentFlight.FlightNotifications == 1 ? true : false;


            base.OnAppearing();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            string dDate1 = departDatePicker.Date.ToString("MM/dd/yyyy");
            string dDate2 = DateTime.Today.Add(departTimePicker.Time).ToString(departTimePicker.Format);

            DateTime dDate3 = Convert.ToDateTime(dDate1 + " " + dDate2);

            string aDate1 = arriveDatePicker.Date.ToString("MM/dd/yyyy");
            string aDate2 = DateTime.Today.Add(arriveTimePicker.Time).ToString(arriveTimePicker.Format);
            DateTime aDate3 = Convert.ToDateTime(aDate1 + " " + aDate2);


            var newFlightInfo = new Flights_Table();
            newFlightInfo.AirlineName = airlineNameEntry.Text;
            newFlightInfo.FlightNumber = flightNumberEntry.Text;
            newFlightInfo.DepartGate = departGateEntry.Text;
            newFlightInfo.DepartLocation = departLocEntry.Text;
            newFlightInfo.DepartTime = dDate3;
            newFlightInfo.ArriveLocation = arriveLocEntry.Text;
            newFlightInfo.ArriveTime = aDate3; 
            newFlightInfo.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            if (newFlightInfo.DepartLocation.Length <= 3 || newFlightInfo.ArriveLocation.Length <= 3)
            {
                if (newFlightInfo.DepartTime <= newFlightInfo.ArriveTime)
                {
                    conn.Update(newFlightInfo);
                    await DisplayAlert("Notice", $"{currentFlight.AirlineName}" + " " + $"{currentFlight.FlightNumber}" + " Updated", "Ok");

                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Warning", "Arrival Date and Time cannot be before Departure Date and Time", "Ok");
                } 
            }
            else
            {
                await DisplayAlert("Warning", "Airport code may only contain 3 letters", "Ok");
            }
        }

        private void NotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {

            currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            conn.Update(currentFlight);
        }
    }
}