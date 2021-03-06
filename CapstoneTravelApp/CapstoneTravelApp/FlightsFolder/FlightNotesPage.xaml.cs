﻿using CapstoneTravelApp.DatabaseTables;
using SQLite;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.FlightsFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlightNotesPage : ContentPage
    {
        private Flights_Table currentFlight;
        private SQLiteConnection conn;
        public FlightNotesPage(Flights_Table _currentFlight)
        {
            InitializeComponent();
            currentFlight = _currentFlight;
            conn = DependencyService.Get<ITravelApp_db>().GetConnection();

            Title = $"{currentFlight.AirlineName}" + " " + $"{ currentFlight.FlightNumber}" + " Notes";

            courseNotesEditor.Text = $"{currentFlight.FlightNotes}";
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Options", "Cancel", null, "Save Notes", "Share Notes");
            if (action == "Save Notes")
            {
                currentFlight.FlightNotes = courseNotesEditor.Text;

                conn.Update(currentFlight);
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