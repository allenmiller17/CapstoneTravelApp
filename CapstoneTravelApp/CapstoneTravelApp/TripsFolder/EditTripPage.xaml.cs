using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.HelperFolders;

namespace CapstoneTravelApp.TripsFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTripPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table _currentTrip;
        public EditTripPage(Trips_Table CurrentTrip)
        {
            InitializeComponent();
            _currentTrip = CurrentTrip;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
            Title = _currentTrip.TripName;
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Trips_Table>();
            tripNameEntry.Text = _currentTrip.TripName;
            startDatePicker.Date = _currentTrip.TripStart;
            endDatePicker.Date = _currentTrip.TripEnd;
            notificationsSwitch.IsToggled = _currentTrip.TripNotifications == 1 ? true : false;


            base.OnAppearing();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            _currentTrip.TripName = tripNameEntry.Text;
            _currentTrip.TripStart = startDatePicker.Date;
            _currentTrip.TripEnd = endDatePicker.Date;
            _currentTrip.TripNotifications = notificationsSwitch.IsToggled == true ? 1 : 0;

            if (UserHelper.IsNull(tripNameEntry.Text))
            {
                if (_currentTrip.TripStart <= _currentTrip.TripEnd)
                {
                    conn.Update(_currentTrip);
                    await DisplayAlert("Notice", $"{_currentTrip.TripName}" + " Updated.", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Warning", "Start date must come before end date", "Ok");
                }
            }
            else await DisplayAlert("Warning", "Trip Name is required", "Ok");
        }
    }
}