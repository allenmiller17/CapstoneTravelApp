using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripNotesPage : ContentPage
    {
        private SQLiteConnection conn;
        private Trips_Table _currentTrip;

        public TripNotesPage(Trips_Table CurrentTrip)
        {
            InitializeComponent();
            _currentTrip = CurrentTrip;
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = _currentTrip.TripName + " Notes";

            NotesEditor.Text = _currentTrip.Notes;
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                _currentTrip.Notes = NotesEditor.Text;

                conn.Update(_currentTrip);
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