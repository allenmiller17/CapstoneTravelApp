using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.HelperFolders;

namespace CapstoneTravelApp.FlightsFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFlightPage : ContentPage
    {
        private Flights_Table currentFlight;
        private SQLiteConnection conn;

        public EditFlightPage(Flights_Table _currentFlight)
        {
            InitializeComponent();
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


            currentFlight.AirlineName = airlineNameEntry.Text;
            currentFlight.FlightNumber = flightNumberEntry.Text;
            currentFlight.DepartGate = departGateEntry.Text;
            currentFlight.DepartLocation = departLocEntry.Text;
            currentFlight.DepartTime = dDate3;
            currentFlight.ArriveLocation = arriveLocEntry.Text;
            currentFlight.ArriveTime = aDate3;
            currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            if (UserHelper.IsNull(airlineNameEntry.Text) || UserHelper.IsNull(flightNumberEntry.Text))
            {
                if (currentFlight.DepartLocation.Length <= 3 || currentFlight.ArriveLocation.Length <= 3)
                {
                    if (currentFlight.DepartTime <= currentFlight.ArriveTime)
                    {
                        conn.Update(currentFlight);
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
            else await DisplayAlert("Warning", "Airline Name and Flight Number are required", "Ok");
        }

        private void NotificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {

            currentFlight.FlightNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            conn.Update(currentFlight);
        }
    }
}