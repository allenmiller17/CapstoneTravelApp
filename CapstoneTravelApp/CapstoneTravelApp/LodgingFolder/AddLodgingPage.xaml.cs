using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.LodgingFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLodging : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table currentTrip;

        public AddLodging(Trips_Table _currentTrip)
        {
            InitializeComponent();
            currentTrip = _currentTrip;

            Title = "Add Lodging";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string cInDate = checkInDatePicker.Date.ToString("MM/dd/yyyy");
            string cInTime = DateTime.Today.Add(checkInTimePicker.Time).ToString(checkInTimePicker.Format);
            DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

            string cOutDate = checkOutDatePicker.Date.ToString("MM/dd/yyyy");
            string cOutTime = DateTime.Today.Add(checkOutTimePicker.Time).ToString(checkOutTimePicker.Format);
            DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

            conn.CreateTable<Lodging_Table>();
            var newLodging = new Lodging_Table();
            newLodging.LodgeName = lodgeNameEntry.Text;
            newLodging.LodgeLocation = lodgeLocEntry.Text;
            newLodging.LodgePhone = lodgePhoneEntry.Text;
            newLodging.LodgeStart = cInDateFull;
            newLodging.LodgeEnd = cOutFull;
            newLodging.LodgeNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            newLodging.TripId = currentTrip.TripId;

            if ((UserHelper.IsNull(lodgeNameEntry.Text)))
            {
                if (newLodging.LodgeStart <= newLodging.LodgeEnd)
                {
                    if (lodgePhoneEntry.Text != null)
                    {
                        if (UserHelper.PhoneCheck(lodgePhoneEntry.Text))
                        {
                            conn.Insert(newLodging);
                            await DisplayAlert("Notice", "Lodging record created", "Ok");
                            await Navigation.PopModalAsync();
                        }
                        else
                        {
                            await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
                        }
                    }
                    else
                    {
                        conn.Insert(newLodging);
                        await DisplayAlert("Notice", "Lodging record created", "Ok");
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Warning", "Check-in must be earlier than check-out", "Ok");
                } 
            }
            else await DisplayAlert("Warning", "Lodging Name is required", "Ok");
        }
    }
}