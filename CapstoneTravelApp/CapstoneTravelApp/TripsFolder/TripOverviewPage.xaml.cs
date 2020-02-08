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

namespace CapstoneTravelApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TripOverviewPage : ContentPage
	{
        private SQLiteConnection conn;
        private Trips_Table CurrentTrip;

		public TripOverviewPage (Trips_Table trip)
		{
			InitializeComponent ();

            CurrentTrip = trip;

            Title = $"{CurrentTrip.TripName}";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();


		}

        protected override void OnAppearing()
        {

            base.OnAppearing();
        }

        private async void FlightsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FlightSelectPage(CurrentTrip));
        }

        private async void LodgingButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HotelSelectPage(CurrentTrip));
        }

        private async void TransportButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransportSelectPage(CurrentTrip));
        }

        private async void DiningButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DiningSelectPage(CurrentTrip));
        }

        private async void EntertainButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntertainSelectPage(CurrentTrip));
        }

        private async void NotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripNotesPage(CurrentTrip));
        }
    }
}