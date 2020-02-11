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
using Xamarin.Essentials;

namespace CapstoneTravelApp.FlightsFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlightNotesPage : ContentPage
	{
        private Flights_Table currentFlight;
        private SQLiteConnection conn;
		public FlightNotesPage (Flights_Table _currentFlight)
		{
			InitializeComponent ();
            currentFlight = _currentFlight;

            Title = $"{currentFlight.AirlineName}" + " " + $"{ currentFlight.FlightNumber}" + " Notes";

            courseNotesEditor.Text = $"{currentFlight.FlightNotes}";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                conn.Insert(currentFlight);
                await DisplayAlert("Notice", "Flight Notes Saved", "Ok");
                await Navigation.PopModalAsync();
            }
            else if (action == "Share Notes")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = courseNotesEditor.Text + "\n" + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
        }
    }
}