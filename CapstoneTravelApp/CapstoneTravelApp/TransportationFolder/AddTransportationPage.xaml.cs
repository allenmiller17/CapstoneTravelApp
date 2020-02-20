using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.TransportationFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTransportationPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table currentTrip;

        public AddTransportationPage(Trips_Table _currentTrip)
        {
            InitializeComponent();

            currentTrip = _currentTrip;
            Title = "Add Transportation";
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string cInDate = pickupDatePicker.Date.ToString("MM/dd/yyyy");
            string cInTime = DateTime.Today.Add(pickupTimePicker.Time).ToString(pickupTimePicker.Format);
            DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

            string cOutDate = dropOffDatePicker.Date.ToString("MM/dd/yyyy");
            string cOutTime = DateTime.Today.Add(dropOffTimePicker.Time).ToString(dropOffTimePicker.Format);
            DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

            var newRental = new Transportation_Table();
            newRental.RentalName = rentalNameEntry.Text;
            newRental.ConfNumber = rentalConfEntry.Text;
            newRental.RentalPhone = rentalPhoneEntry.Text;
            newRental.PickUpDate = cInDateFull;
            newRental.PickUpLocation = pickupLocEntry.Text;
            newRental.ReturnDate = cOutFull;
            newRental.ReturnLocation = dropOffLocEntry.Text;
            newRental.RentalNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            newRental.TripId = currentTrip.TripId;

            if ((UserHelper.IsNull(rentalNameEntry.Text) || UserHelper.IsNull(rentalConfEntry.Text)))
            {
                if (newRental.PickUpDate <= newRental.ReturnDate)
                {
                    if (UserHelper.PhoneCheck(rentalPhoneEntry.Text))
                    {
                        conn.Insert(newRental);
                        await DisplayAlert("Notice", "Transportation record added", "Ok");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Warning", "Pickup must be earlier than Drop off", "Ok");
                } 
            }
            else await DisplayAlert("Warning", "Rental Name and Confirmation Number are required", "Ok");
        }
    }
}