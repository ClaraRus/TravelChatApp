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
    public class Place
    {
       
        private string status;
        private List<Candidate> candidates;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public List<Candidate> Candidates
        {
            get { return candidates; }
            set { candidates = value; }
        }



    }
}