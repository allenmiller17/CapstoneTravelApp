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
using CapstoneTravelApp.TransportationFolder;

namespace CapstoneTravelApp.TransportationFolder
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransportationInfoPage : ContentPage
	{
		public TransportationInfoPage (Transportation_Table currentRental)
		{
			InitializeComponent ();
		}
	}
}