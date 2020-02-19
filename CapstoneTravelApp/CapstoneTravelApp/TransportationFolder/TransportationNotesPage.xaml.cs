using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.TransportationFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransportationNotesPage : ContentPage
    {
        private SQLiteConnection conn;
        private Transportation_Table currentRental;

        public TransportationNotesPage(Transportation_Table _currentRental)
        {
            InitializeComponent();
            currentRental = _currentRental;
            Title = $"{currentRental.RentalName}" + " Notes";

            transportNotesEditor.Text = $"{currentRental.RentalNotes}";

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                currentRental.RentalNotes = transportNotesEditor.Text;

                conn.Update(currentRental);
                await DisplayAlert("Notice", "Transportation Notes Saved", "Ok");
                await Navigation.PopModalAsync();
            }
            else if (action == "Share Notes")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = transportNotesEditor.Text + "\n" + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
        }
    }
}