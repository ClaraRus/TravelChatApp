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
    public class Review
    {
        public string Language { get; set; }
        public string Text { get; set; }
        public string Rating { get; set; }
        public string Time { get; set; }
       
    }
}