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
	public partial class EntertainmentInfoPage : ContentPage
	{
		private SQLiteConnection conn;
		private Entertainment_Table _currentActivity;
		private ObservableCollection<Entertainment_Table> _activityList;

		public EntertainmentInfoPage (Entertainment_Table currentActivity)
		{
			InitializeComponent ();
			_currentActivity = currentActivity;
			Title = _currentActivity.EntertainName;
			conn = DependencyService.Get<ITravelApp_db>().GetConnection();

		}

		protected async override void OnAppearing()
		{
			conn.CreateTable<Entertainment_Table>();
			var activityList = conn.Query<Entertainment_Table>($"SELECT * FROM Entertainment_Table WHERE TripId = '{_currentActivity.TripId}'");
			_activityList = new ObservableCollection<Entertainment_Table>(activityList);

			activityNameLabel.Text = _currentActivity.EntertainName;
			activityLocLabel.Text = _currentActivity.EnterainAddress;
			
			base.OnAppearing();
		}

		private void menuButton_Clicked(object sender, EventArgs e)
		{

		}
	}
}