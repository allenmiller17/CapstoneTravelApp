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
using CapstoneTravelApp.FlightsFolder;


namespace CapstoneTravelApp.LodgingFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LodgingNotesPage : ContentPage
	{
        private SQLiteConnection conn;
        private Lodging_Table lodging;
		public LodgingNotesPage (Lodging_Table _currentLodging)
		{
			InitializeComponent ();
            lodging = _currentLodging;
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = $"{lodging.LodgeName}" + " Notes";

            courseNotesEditor.Text = $"{lodging.LodgeNotes}";
		}

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                lodging.LodgeNotes = courseNotesEditor.Text;

                conn.Update(lodging);
                await DisplayAlert("Notice", "Lodging Notes Saved", "Ok");
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