using CapstoneTravelApp.DatabaseTables;
using CapstoneTravelApp.HelperFolders;
using SQLite;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CapstoneTravelApp.EntertainmentFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddEntertainmentPage : ContentPage
	{
		private SQLiteConnection conn;
		private Trips_Table currentTrip;

		public AddEntertainmentPage(Trips_Table _currentTrip)
		{
			InitializeComponent();
			currentTrip = _currentTrip;

			Title = "Add Entertainment";

			conn = DependencyService.Get<ITravelApp_db>().GetConnection();
		}

		private async void SaveButton_Clicked(object sender, EventArgs e)
		{
			string cInDate = startDatePicker.Date.ToString("MM/dd/yyyy");
			string cInTime = DateTime.Today.Add(startTimePicker.Time).ToString(startTimePicker.Format);
			DateTime cInDateFull = Convert.ToDateTime(cInDate + " " + cInTime);

			string cOutDate = endDatePicker.Date.ToString("MM/dd/yyyy");
			string cOutTime = DateTime.Today.Add(endTimePicker.Time).ToString(endTimePicker.Format);
			DateTime cOutFull = Convert.ToDateTime(cOutDate + " " + cOutTime);

			conn.CreateTable<Entertainment_Table>();
			var entertain = new Entertainment_Table();
			entertain.TripId = currentTrip.TripId;
			entertain.EntertainName = entertainNameEntry.Text;
			entertain.EnterainAddress = entertainLocEntry.Text;
			entertain.EntertainPhone = entertainPhoneEntry.Text;
			entertain.EntertaninStart = cInDateFull;
			entertain.EntertainEnd = cOutFull;
			entertain.EntertainNotifications = notificationSwitch.IsToggled == true ? 1 : 0;

			if (entertain.EntertaninStart <= entertain.EntertainEnd)
			{
				if (UserHelper.PhoneCheck(entertainPhoneEntry.Text))
				{
					conn.Insert(entertain);
					await DisplayAlert("Notice", "Entertainment record created", "Ok");
					await Navigation.PopModalAsync();
				}
				else
				{
					await DisplayAlert("Warning", "Phone Number may only be 10 digits and only contain numbers", "Ok");
				}
			}
			else
			{
				await DisplayAlert("Warning", "Check-in must be earlier than check-out", "Ok");
			}
		}
	}
}