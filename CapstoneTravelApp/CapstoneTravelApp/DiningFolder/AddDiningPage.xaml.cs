using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.DiningFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDiningPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table currentTrip;

        public AddDiningPage(Trips_Table _currentTrip)
        {
            InitializeComponent();
            currentTrip = _currentTrip;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = "Add Dining Reservation";
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string cInDate = DiningDatePicker.Date.ToString("MM/dd/yyyy");
            string cInTime = DateTime.Today.Add(DiningTimePicker.Time).ToString(DiningTimePicker.Format);
            DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

            conn.CreateTable<Dining_Table>();
            var currentRes = new Dining_Table();
            currentRes.ResName = DiningNameEntry.Text;
            currentRes.ResAddress = DiningLocEntry.Text;
            currentRes.ResPhone = DiningPhoneEntry.Text;
            currentRes.ResDate = cInDateFull;
            currentRes.ResNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            currentRes.TripId = currentTrip.TripId;

            if (UserHelper.PhoneCheck(DiningPhoneEntry.Text))
            {
                conn.Insert(currentRes);
                await DisplayAlert("Notice", "Dining Reservation created", "Ok");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
            }
        }
    }
}