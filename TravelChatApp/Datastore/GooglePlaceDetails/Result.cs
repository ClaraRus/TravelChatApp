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
    public class Result
    {
        public string Formatted_address { get; set; }
        public string Name { get; set; }
        public string Place_id { get; set; }
        public double Rating { get; set; }
        public List<Review> Reviews { get; set; }
    }
}