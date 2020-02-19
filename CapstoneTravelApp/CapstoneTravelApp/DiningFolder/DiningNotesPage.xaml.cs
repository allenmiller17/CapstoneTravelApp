using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.DiningFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiningNotesPage : ContentPage
    {
        private SQLiteConnection conn;
        private Dining_Table currentRes;

        public DiningNotesPage(Dining_Table _currentRes)
        {
            InitializeComponent();
            currentRes = _currentRes;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = $"{currentRes.ResName}" + " Notes";
            NotesEditor.Text = currentRes.ResNotes;
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                currentRes.ResNotes = NotesEditor.Text;

                conn.Update(currentRes);
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