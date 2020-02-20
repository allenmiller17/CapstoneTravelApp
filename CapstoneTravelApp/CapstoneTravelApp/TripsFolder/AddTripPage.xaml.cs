
using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.TripsFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTripPage : ContentPage
    {
        private SQLiteConnection conn;
        private string _currentUser;
        public AddTripPage(string CurrentUser)
        {
            InitializeComponent();

            _currentUser = CurrentUser;

            Title = "Add New Trip";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            conn.CreateTable<Trips_Table>();
        }

        private async void SaveButton_Clicked(object sender, System.EventArgs e)
        {

            var newTrip = new Trips_Table();
            newTrip.TripName = tripNameEntry.Text;
            newTrip.TripStart = startDatePicker.Date;
            newTrip.TripEnd = endDatePicker.Date;
            newTrip.TripNotifications = notificationsSwitch.IsToggled == true ? 1 : 0;
            newTrip.UserName = _currentUser;

            if ((UserHelper.IsNull(tripNameEntry.Text)))
            {
                if (newTrip.TripStart <= newTrip.TripEnd)
                {
                    conn.Insert(newTrip);
                    await DisplayAlert("Notice", "New Trip Added", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("Warning", "Start Date must be earlier than End Date", "Ok"); 
            }
            else await DisplayAlert("Warning", "Trip Name is required", "Ok");
        }
    }
}