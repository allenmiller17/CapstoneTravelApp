using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.LodgingFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditLodging : ContentPage
    {
        private SQLiteConnection conn;
        private Lodging_Table _hotel;

        public EditLodging(Lodging_Table hotel)
        {
            InitializeComponent();
            _hotel = hotel;

            Title = hotel.LodgeName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Lodging_Table>();
            lodgeNameEntry.Text = _hotel.LodgeName;
            lodgeLocEntry.Text = _hotel.LodgeLocation;
            lodgePhoneEntry.Text = _hotel.LodgePhone;
            checkInDatePicker.Date = _hotel.LodgeStart;
            checkInTimePicker.Time = _hotel.LodgeStart.TimeOfDay;
            checkOutDatePicker.Date = _hotel.LodgeEnd;
            checkOutTimePicker.Time = _hotel.LodgeEnd.TimeOfDay;
            notificationSwitch.IsToggled = _hotel.LodgeNotifications == 1 ? true : false;


            base.OnAppearing();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string cInDate = checkInDatePicker.Date.ToString("MM/dd/yyyy");
            string cInTime = DateTime.Today.Add(checkInTimePicker.Time).ToString(checkInTimePicker.Format);
            DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

            string cOutDate = checkOutDatePicker.Date.ToString("MM/dd/yyyy");
            string cOutTime = DateTime.Today.Add(checkOutTimePicker.Time).ToString(checkOutTimePicker.Format);
            DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

            _hotel.LodgeName = lodgeNameEntry.Text;
            _hotel.LodgeLocation = lodgeLocEntry.Text;
            _hotel.LodgePhone = lodgePhoneEntry.Text;
            _hotel.LodgeStart = cInDateFull;
            _hotel.LodgeEnd = cOutFull;
            _hotel.LodgeNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            if (_hotel.LodgeStart <= _hotel.LodgeEnd)
            {
                if (UserHelper.PhoneCheck(lodgePhoneEntry.Text))
                {
                    conn.Update(_hotel);
                    await DisplayAlert("Notice", "Lodging record updated", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Warning", "Check-in must be earlier than check-out", "Ok");
            }
        }
    }
}