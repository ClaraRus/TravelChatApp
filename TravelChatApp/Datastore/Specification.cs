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
    public class Candidate
    {
        private string formatted_address;
        private string name;
        //private string opening_hours;
        private string rating;

        //private List<Review> reviews;

        public string Formatted_Address
        {
            get { return formatted_address; }
            set { formatted_address = value; }
        }

        /*public string Opening_Hours
        {
            get { return opening_hours; }
            set { opening_hours = value; }
        }*/

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }
    }
}