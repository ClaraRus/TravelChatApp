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
	public class Suggestion
	{
		public string[] text;
		
		public string [] Text
		{
			get { return text; }
			set { text = value; }
		}

public bool IsButton { set; get; }
	}
}