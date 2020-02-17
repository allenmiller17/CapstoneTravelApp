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
using CapstoneTravelApp.EntertainmentFolder;
using Xamarin.Essentials;

namespace CapstoneTravelApp.EntertainmentFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntertainmentNotesPage : ContentPage
	{
        private SQLiteConnection conn;
        private Entertainment_Table currentActivity;

		public EntertainmentNotesPage (Entertainment_Table _currentActivity)
		{
			InitializeComponent ();

            currentActivity = _currentActivity;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = _currentActivity.EntertainName + " Notes";

            NotesEditor.Text = currentActivity.EntertainNotes;
		}

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                currentActivity.EntertainNotes = NotesEditor.Text;

                conn.Update(currentActivity);
                await DisplayAlert("Notice", "Notes Saved", "Ok");
                await Navigation.PopModalAsync();
            }
            else if (action == "Share Notes")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = NotesEditor.Text + "\n" + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
        }
    }
}