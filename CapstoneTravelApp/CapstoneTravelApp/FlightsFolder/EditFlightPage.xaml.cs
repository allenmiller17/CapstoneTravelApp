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
            //departDatePicker.Format = DatePicker.Format.Custom;

            //string dDate1 = departDatePicker.Date.ToString("MM/dd/yyyy") + departTimePicker.Time.ToString("hh:mm tt");
            //string dDate2 = departTimePicker.ToString();
            //DateTime dDate3 = Convert.ToDateTime(dDate1 + " " + dDate2);
            //DateTime dDate = DateTime.ParseExact(dDate1 + " " + dDate2, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            //string aDate1 = arriveDatePicker.Date.ToString("MM/dd/yyyy");
            //string aDate2 = arriveTimePicker.Time.ToString("hh:mm:ss tt");
            ////DateTime aDate3 = Convert.ToDateTime(aDate1 + " " + aDate2);
            //DateTime aDate = DateTime.ParseExact(aDate1 + " " + aDate2, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            var newFlightInfo = new Flights_Table();
            newFlightInfo.AirlineName = airlineNameEntry.Text;
            newFlightInfo.FlightNumber = flightNumberEntry.Text;
            newFlightInfo.DepartGate = departGateEntry.Text;
            newFlightInfo.DepartLocation = departLocEntry.Text;
            newFlightInfo.DepartTime = departDatePicker.Date;
            newFlightInfo.ArriveLocation = arriveLocEntry.Text;
            newFlightInfo.ArriveTime = arriveDatePicker.Date; ;
            newFlightInfo.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            if (newFlightInfo.DepartTime < newFlightInfo.ArriveTime)
            {
                conn.Insert(newFlightInfo);
                await DisplayAlert("Notice", "New Flight Added to Trip", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Warning", "Arrival Date or Time cannot be before Departure Date and Time", "Ok");
            }
        }

        private void NotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {

            currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            conn.Update(currentFlight);
        }
    }
}