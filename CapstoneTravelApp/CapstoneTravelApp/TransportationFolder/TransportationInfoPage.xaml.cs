using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CapstoneTravelApp.DatabaseTables;
using SQLite;
using CapstoneTravelApp.HelperFolders;
using System.Collections.ObjectModel;
using CapstoneTravelApp.TransportationFolder;
using Xamarin.Essentials;

namespace CapstoneTravelApp.TransportationFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransportationInfoPage : ContentPage
	{
        private SQLiteConnection conn;
        private Transportation_Table _currentRental;
        private ObservableCollection<Transportation_Table> rentalList;
		public TransportationInfoPage (Transportation_Table currentRental)
		{
			InitializeComponent ();

            _currentRental = currentRental;

            Title = _currentRental.RentalName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            //var tapGestureRecognizer = new TapGestureRecognizer();
            //tapGestureRecognizer.Tapped += (s, e) => { Command cmd = new Command(() => { UserHelper.PlacePhoneCall(transportationPhoneLabel.Text); }); };


		}

        protected async override void OnAppearing()
        {
            conn.CreateTable<Transportation_Table>();
            var _rentalList = conn.Query<Transportation_Table>($"SELECT * FROM Transportation_Table WHERE TripId = '{_currentRental.TripId}'");
            rentalList = new ObservableCollection<Transportation_Table>(_rentalList);

            rentalNameLabel.Text = _currentRental.RentalName;
            confNumberLabel.Text = _currentRental.ConfNumber;
            transportationPhoneLabel.Text = _currentRental.RentalPhone;
            pickupLocLabel.Text = _currentRental.PickUpLocation;
            pickupDateLabel.Text = _currentRental.PickUpDate.ToString("MM/dd hh:mm tt");
            returnDateLabel.Text = _currentRental.ReturnDate.ToString("MM/dd hh:mm tt");
            dropoffLocLabel.Text = _currentRental.ReturnLocation;
            notificationSwitch.IsToggled = _currentRental.RentalNotifications == 1 ? true : false;


            //Open phone number in phone app
            transportationPhoneLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    UserHelper.PlacePhoneCall(transportationPhoneLabel.Text);
                })
            });

            //Open Address or landmark in maps
            var address = pickupLocLabel.Text;
            var location = await Geocoding.GetLocationsAsync(address);
            var _location = location?.FirstOrDefault();

            pickupLocLabel.GestureRecognizers.Add(new TapGestureRecognizer()
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

                        DisplayAlert("Warning", "This function is not currently available","Ok");
                    }
                })
            });

            //Dropoff Navigation
            var address1 = dropoffLocLabel.Text;
            var location1 = await Geocoding.GetLocationsAsync(address);
            var _location1 = location?.FirstOrDefault();

            dropoffLocLabel.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    try
                    {
                        if (location1 != null)
                        {
                            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
                            Map.OpenAsync(_location1.Latitude, _location1.Longitude, options);
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

        private async void TransportationNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TransportationNotesPage(_currentRental));
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Lodging Options", "Cancel", "Delete Transportation", "Edit Transportation", "Share Transportation");
            if (action == "Edit Transportation")
            {
                await Navigation.PushModalAsync(new EditTransportationPage(_currentRental));
            }
            else if (action == "Share Transportation")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = rentalNameLabel.Text + "\n" + confNumberLabel.Text + "\n" + transportationPhoneLabel.Text +
                    "\n" + "Pickup: " + pickupLocLabel.Text + "\n" + pickupDateLabel.Text + "\n" +
                    "Dropoff: " + dropoffLocLabel.Text + "\n" + returnDateLabel.Text + "\n"
                    + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
            else if (action == "Delete Transportation")
            {
                var deleteResponse = await DisplayAlert("Warning", "You are about to delete this transportation record! Are you sure?", "Yes", "No");
                if (deleteResponse)
                {
                    conn.Delete(_currentRental);
                    await Navigation.PopAsync();
                }
            }
        }

        private void NotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            conn.CreateTable<Transportation_Table>();
            _currentRental.RentalNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentRental);
        }
    }
}