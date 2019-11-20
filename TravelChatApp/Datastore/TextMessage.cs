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
    class TextMessage
    {
        private string text;
        private string sender;

        public TextMessage(string sender, string text)
        {
            this.sender = sender;
            this.text = text;
        }

        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
    }
}