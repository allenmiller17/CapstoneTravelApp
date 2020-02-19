using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.TransportationFolder;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransportSelectPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table _currentTrip;
        public ObservableCollection<Transportation_Table> rentalList;

        public TransportSelectPage(Trips_Table CurrentTrip)
        {
            InitializeComponent();

            _currentTrip = CurrentTrip;
            Title = "Select Local Transportation";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            TransportListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Rental_Tapped);
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Transportation_Table>();
            var _rentalList = conn.Query<Transportation_Table>($"SELECT * FROM Transportation_Table WHERE TripId = '{_currentTrip.TripId}'");
            rentalList = new ObservableCollection<Transportation_Table>(_rentalList);

            TransportListView.ItemsSource = rentalList.OrderBy(d => d.PickUpDate);

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Add Transportation", "Share Upcoming Transportation");
            if (action == "Add Transportation")
            {
                await Navigation.PushModalAsync(new AddTransportationPage(_currentTrip));
            }
            else if (action == "Share Upcoming Lodgin")
            {
                foreach (Transportation_Table rental in rentalList)
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Text = rental.RentalName + " " + rental.RentalPhone + "\n" +
                        rental.PickUpLocation + "\n" +
                        "Pickup Time: " + rental.PickUpDate + "\n" +
                         rental.ReturnLocation + "\n" +
                        "Return Time: " + rental.ReturnDate +
                        "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt") + "\n" + "\n" + "\n"
                    });
                }
            }
        }
        private async void Rental_Tapped(object sender, ItemTappedEventArgs e)
        {
            Transportation_Table currentRental = (Transportation_Table)e.Item;
            await Navigation.PushAsync(new TransportationInfoPage(currentRental));
        }
    }
}