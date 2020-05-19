using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TravelChatApp.Datastore
{
	class PlaceInformation
	{
		public string Location { set; get; }
		public string Title { set; get; }
		public string Content { set; get; }
		public double Rating { set; get; }
		public string Sentiment { set; get; }
	}
}