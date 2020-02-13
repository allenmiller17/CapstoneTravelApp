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
using CapstoneTravelApp.LodgingFolder;
using Xamarin.Essentials;

namespace CapstoneTravelApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HotelSelectPage : ContentPage
	{
        private SQLiteConnection conn;
        Trips_Table _currentTrip;
        public ObservableCollection<Lodging_Table> lodgingList;
		public HotelSelectPage (Trips_Table CurrentTrip)
		{
			InitializeComponent ();
            _currentTrip = CurrentTrip;

            Title = "Select Lodging";
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            LodgingListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Hotel_Tapped);
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Lodging_Table>();
            var _lodgingList = conn.Query<Lodging_Table>($"SELECT * FROM Lodging_Table WHERE TripId = '{_currentTrip.TripId}'");
            lodgingList = new ObservableCollection<Lodging_Table>(_lodgingList);

            LodgingListView.ItemsSource = lodgingList.OrderBy(d => d.LodgeStart);

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Add Lodging", "Share Upcoming Lodging");
            if (action == "Add Lodging")
            {
                await Navigation.PushModalAsync(new AddLodging(_currentTrip));
            }
            else if (action == "Share Upcoming Lodgin")
            {
                foreach (Lodging_Table hotel in lodgingList)
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Text = hotel.LodgeName + " " + hotel.LodgePhone + "\n" +
                        hotel.LodgeLocation + "\n" +
                        "Check-in Time: " + hotel.LodgeStart + "\n" +
                        "Arrival Time: " + hotel.LodgeEnd +
                        "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt") + "\n" + "\n" + "\n"
                    });
                }
            }
        }

        private async void Hotel_Tapped (object sender, ItemTappedEventArgs e)
        {
            Lodging_Table currentLodging = (Lodging_Table)e.Item;
            await Navigation.PushAsync(new LodgingInfoPage(currentLodging));
        }
    }
}