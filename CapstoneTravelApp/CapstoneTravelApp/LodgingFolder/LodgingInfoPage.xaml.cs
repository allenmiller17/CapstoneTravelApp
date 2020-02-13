﻿using System;
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
	public partial class LodgingInfoPage : ContentPage
	{
        private SQLiteConnection conn;
        private Lodging_Table _currentLodging;
        private ObservableCollection<Lodging_Table> _LodgingList;
		public LodgingInfoPage (Lodging_Table currentLodging)
		{
			InitializeComponent ();
            _currentLodging = currentLodging;

            Title = _currentLodging.LodgeName;

            conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

        protected override void OnAppearing()
        {
            conn.CreateTable<Lodging_Table>();
            var lodgingList = conn.Query<Lodging_Table>($"SELECT * FROM Lodging_Table WHERE LodgeId = '{_currentLodging.LodgeId}'");
            _LodgingList = new ObservableCollection<Lodging_Table>(lodgingList);

            lodgingNameLabel.Text = _currentLodging.LodgeName;
            lodgingLocLabel.Text = _currentLodging.LodgeLocation;
            lodgingPhoneLabel.Text = _currentLodging.LodgePhone;
            checkInDateLabel.Text = _currentLodging.LodgeStart.ToString("MM/dd hh:mm tt");
            checkOutDateLabel.Text = _currentLodging.LodgeEnd.ToString("MM/dd hh:mm tt");
            notificationSwitch.IsToggled = _currentLodging.LodgeNotifications == 1 ? true : false;

            base.OnAppearing();
        }

        private async void MenuButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Lodging Options", "Cancel", "Delete Lodging", "Edit Lodging", "Share Lodging");
            if (action == "Edit Lodging")
            {
                await Navigation.PushModalAsync(new EditLodging(_currentLodging));
            }
            else if (action == "Share Lodging")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = lodgingNameLabel.Text + "\n" + lodgingLocLabel.Text + "\n" + lodgingPhoneLabel.Text +
                    "\n" + checkInDateLabel.Text + "\n"
                    + "Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt")
                });
            }
            else if (action == "Delete Lodging")
            {
                var deleteResponse = await DisplayAlert("Warning", "You are about to delete this lodging record! Are you sure?", "Yes", "No");
                if (deleteResponse)
                {
                    conn.Delete(_currentLodging);
                    await Navigation.PopAsync();
                }
            }
        }

        private async void LodgingNotesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LodgingNotesPage(_currentLodging));
        }

        private void NotificationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            conn.CreateTable<Lodging_Table>();
            _currentLodging.LodgeNotifications = notificationSwitch.IsToggled == true ? 1 : 0;
            conn.Update(_currentLodging);
        }
    }
}