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

namespace CapstoneTravelApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntertainSelectPage : ContentPage
	{
		private SQLiteConnection conn;
		Trips_Table _currentTrip;
		private ObservableCollection<Entertainment_Table> activityList;

		public EntertainSelectPage (Trips_Table CurrentTrip)
		{
			InitializeComponent ();
			_currentTrip = CurrentTrip;

			Title = "Select Entertainment";

			conn = DependencyService.Get<ITravelApp_db>().GetConnection();

			EntertainmentListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Activity_Tapped);
		}

		protected override void OnAppearing()
		{
			conn.CreateTable<Entertainment_Table>();
			var _activityList = conn.Query<Entertainment_Table>($"SELECT * FROM Entertainment_Table WHERE TripId = '{_currentTrip.TripId}'");
			activityList = new ObservableCollection<Entertainment_Table>(_activityList);

			EntertainmentListView.ItemsSource = activityList.OrderBy(d => d.EntertaninStart);

			base.OnAppearing();
		}

		private async void menuButton_Clicked(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet("Options", "Cancel", null, "Add Entertainment", "Share Upcoming Entertainment");
			if (action == "Add Entertainment")
			{
				await Navigation.PushModalAsync(new AddEntertainmentPage(_currentTrip));
			}
			else if (action == "Share Upcoming Entertainment")
			{
				foreach (Entertainment_Table activity in activityList)
				{
					await Share.RequestAsync(new ShareTextRequest
					{
						Text = activity.EntertainName + " " + activity.EntertainPhone + "\n" +
						activity.EnterainAddress + "\n" +
						"Start Time: " + activity.EntertaninStart + "\n" +
						"End Time: " + activity.EntertainEnd +
						"Record updated at: " + DateTime.Now.ToString("MM/dd/yy HH:mm tt") + "\n" + "\n" + "\n"
					});
				}
			}
		}

		private async void Activity_Tapped(object sender, ItemTappedEventArgs e)
		{
			Entertainment_Table currentActivity = (Entertainment_Table)e.Item;
			await Navigation.PushAsync(new EntertainmentInfoPage(currentActivity));
		}
	}
}