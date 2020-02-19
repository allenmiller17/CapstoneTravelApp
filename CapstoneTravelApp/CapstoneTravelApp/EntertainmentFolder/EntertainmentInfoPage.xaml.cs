using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.EntertainmentFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntertainmentInfoPage : ContentPage
    {
        private SQLiteConnection conn;
        private Entertainment_Table _currentActivity;
        private ObservableCollection<Entertainment_Table> _activityList;

        public EntertainmentInfoPage(Entertainment_Table currentActivity)
        {
            InitializeComponent();
            _currentActivity = currentActivity;
            Title = _currentActivity.EntertainName;
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

        }

        protected async override void OnAppearing()
        {
            conn.CreateTable<Entertainment_Table>();
            var activityList = conn.Query<Entertainment_Table>($"SELECT * FROM Entertainment_Table WHERE TripId = '{_currentActivity.TripId}'");
            _activityList = new ObservableCollection<Entertainment_Table>(activityList);

            activityNameLabel.Text = _currentActivity.EntertainName;
            activityLocLabel.Text = _currentActivity.EnterainAddress;
            activityPhoneLabel.Text = _currentActivity.EntertainPhone;
            startDateLabel.Text = _currentActivity.EntertaninStart.ToString("MM/dd hh:mm tt");
            endDateLabel.Text = _currentActivity.EntertainEnd.ToString("MM/dd hh:mm tt");
            notificationSwitch.IsToggled = _currentActivity.EntertainNotifications == 1 ? true : false;

            //Open phone number in phone app
            activityPhoneLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    UserHelper.PlacePhoneCall(activityPhoneLabel.Text);
                })
            });

            //Open Address in maps
            var address = activityLocLabel.Text;
            var location = await Geocoding.GetLocationsAsync(address);
            var _location = location?.FirstOrDefault();

            activityLocLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    try
                    {
                        if (location != null)
                        {
                            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                            Map.OpenAsync(_location.Latitude, _location.Longitude, options);
                        }
                    }
                    catch (Exception)
                    {

                        DisplayAlert("Warning", "This function is not currently available", "Ok");
                    }
                })
            });

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Entertainment Options", "Cancel", "Delete Entertainment", "Edit Entertainment", "Share Entertainment");
            if (action == "Edit Entertainment")
            {
                await Navigation.PushModalAsync(new EditEntertainmentPage(_currentActivity));
            }
            else if (action == "Share Entertainment")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = activityNameLabel.Text + "\n" + activityLocLabel.Text + "\n" + activityPhoneLabel.Text +
                    "\n" + startDateLabel.Text + "\n" + endDateLabel.Text + "\n"
                    + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
            else if (action == "Delete Entertainment")
            {
                var deleteResponse = await DisplayAlert("Warning", "You are about to delete this Entertainment record! Are you sure?", "Yes", "No");
                if (deleteResponse)
                {
                    conn.Delete(_currentActivity);
                    await Navigation.PopAsync();
                }
            }
        }

        private async void ActivityNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EntertainmentNotesPage(_currentActivity));
        }

        private void NotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            conn.CreateTable<Entertainment_Table>();
            _currentActivity.EntertainNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentActivity);
        }
    }
}