using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.DiningFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditDiningPage : ContentPage
    {
        private SQLiteConnection conn;
        private Dining_Table currentRes;

        public EditDiningPage(Dining_Table _currentRes)
        {
            InitializeComponent();
            currentRes = _currentRes;

            Title = currentRes.ResName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Dining_Table>();
            DiningNameEntry.Text = currentRes.ResName;
            DiningLocEntry.Text = currentRes.ResAddress;
            DiningPhoneEntry.Text = currentRes.ResPhone;
            DiningDatePicker.Date = currentRes.ResDate;
            DiningTimePicker.Time = currentRes.ResDate.TimeOfDay;
            notificationSwitch.IsToggled = currentRes.ResNotifications == 1 ? true : false;

            base.OnAppearing();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            string cInDate = DiningDatePicker.Date.ToString("MM/dd/yyyy");
            string cInTime = DateTime.Today.Add(DiningTimePicker.Time).ToString(DiningTimePicker.Format);
            DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

            currentRes.ResName = DiningNameEntry.Text;
            currentRes.ResAddress = DiningLocEntry.Text;
            currentRes.ResPhone = DiningPhoneEntry.Text;
            currentRes.ResDate = cInDateFull;
            currentRes.ResNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

            if (UserHelper.IsNull(DiningNameEntry.Text))
            {

                if (UserHelper.PhoneCheck(DiningPhoneEntry.Text))
                {
                    conn.Update(currentRes);
                    await DisplayAlert("Notice", "Dining Reservation updated", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
                }
            }
            else await DisplayAlert("Warning", "Dining Name is required", "Ok");
        }
    }
}