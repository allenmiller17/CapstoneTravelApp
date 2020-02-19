using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.DiningFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiningInfoPage : ContentPage
    {
        private SQLiteConnection conn;
        private Dining_Table _currentRes;

        public DiningInfoPage(Dining_Table currentDining)
        {
            InitializeComponent();

            _currentRes = currentDining;

            Title = _currentRes.ResName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            conn.CreateTable<Dining_Table>();
            var diningList = conn.Query<Dining_Table>($"SELECT * FROM Dining_Table WHERE TripId = '{_currentRes.TripId}'");

            DiningNameLabel.Text = _currentRes.ResName;
            DiningLocLabel.Text = _currentRes.ResAddress;
            DiningPhoneLabel.Text = _currentRes.ResPhone;
            ResDateLabel.Text = _currentRes.ResDate.ToString("MM/dd hh:mm tt");
            notificationSwitch.IsToggled = _currentRes.ResNotifications == 1 ? true : false;


            //Open phone number in phone app
            DiningPhoneLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    UserHelper.PlacePhoneCall(DiningPhoneLabel.Text);
                })
            });

            ////Open Address in maps
            //var address = DiningLocLabel.Text;
            //var location = await Geocoding.GetLocationsAsync(address);
            //var _location = location?.FirstOrDefault();

            //DiningLocLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            //{
            //    Command = new Command(() =>
            //    {
            //        try
            //        {
            //            if (location != null)
            //            {
            //                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            //                Map.OpenAsync(_location.Latitude, _location.Longitude, options);
            //            }
            //        }
            //        catch (Exception)
            //        {

            //            DisplayAlert("Warning", "This function is not currently available", "Ok");
            //        }
            //    })
            //});

            base.OnAppearing();
        }

        private async void DiningNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DiningNotesPage(_currentRes));
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Dining Options", "Cancel", "Delete Dining", "Edit Dining", "Share Dining");
            if (action == "Edit Dining")
            {
                await Navigation.PushModalAsync(new EditDiningPage(_currentRes));
            }
            else if (action == "Share Dining")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = DiningNameLabel.Text + "\n" + DiningLocLabel.Text + "\n" + DiningPhoneLabel.Text +
                    "\n" + ResDateLabel.Text + "\n"
                    + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
            else if (action == "Delete Dining")
            {
                var deleteResponse = await DisplayAlert("Warning", "You are about to delete this dining reservation! Are you sure?", "Yes", "No");
                if (deleteResponse)
                {
                    conn.Delete(_currentRes);
                    await Navigation.PopAsync();
                }
            }
        }

        private void NotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            conn.CreateTable<Dining_Table>();
            _currentRes.ResNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentRes);
        }
    }
}