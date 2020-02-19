using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.DiningFolder;
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
    public partial class DiningSelectPage : ContentPage
    {
        private SQLiteConnection conn;
        Trips_Table _currentTrip;
        public ObservableCollection<Dining_Table> diningList;

        public DiningSelectPage(Trips_Table CurrentTrip)
        {
            InitializeComponent();
            _currentTrip = CurrentTrip;

            Title = "Select Dining";
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            DiningListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Dining_Tapped);
        }

        protected override void OnAppearing()
        {
            conn.CreateTable<Dining_Table>();
            var _diningList = conn.Query<Dining_Table>($"SELECT * FROM Dining_Table WHERE TripId = '{_currentTrip.TripId}'");
            diningList = new ObservableCollection<Dining_Table>(_diningList);

            DiningListView.ItemsSource = diningList.OrderBy(d => d.ResDate);

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Add Dining", "Share Upcoming Dining");
            if (action == "Add Dining")
            {
                await Navigation.PushModalAsync(new AddDiningPage(_currentTrip));
            }
            else if (action == "Share Upcoming Dining")
            {
                foreach (Dining_Table dining in diningList)
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Text = dining.ResName + " " + dining.ResPhone + "\n" +
                        dining.ResAddress + "\n" +
                        "Time: " + dining.ResDate + "\n" +
                        "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt") + "\n" + "\n" + "\n"
                    });
                }
            }
        }

        private async void Dining_Tapped(object sender, ItemTappedEventArgs e)
        {
            Dining_Table currentDining = (Dining_Table)e.Item;
            await Navigation.PushAsync(new DiningInfoPage(currentDining));
        }
    }
}