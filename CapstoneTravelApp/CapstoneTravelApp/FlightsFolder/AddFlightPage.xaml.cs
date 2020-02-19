using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFlightPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table currentTrip;

        public AddFlightPage(Trips_Table _currentTrip)
        {
            InitializeComponent();
            Title = "Add Flight";
            currentTrip = _currentTrip;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            conn.CreateTable<Flights_Table>();
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
            newFlightInfo.TripId = currentTrip.TripId;

            if (newFlightInfo.DepartLocation.Length <= 3 || newFlightInfo.ArriveLocation.Length <= 3)
            {
                if (newFlightInfo.DepartTime <= newFlightInfo.ArriveTime)
                {
                    conn.Insert(newFlightInfo);
                    await DisplayAlert("Notice", "Flight added!", "Ok");

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
    }
}