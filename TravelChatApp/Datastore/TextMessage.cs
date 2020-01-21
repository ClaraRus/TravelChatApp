using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TravelChatApp.Datastore
{
    public class TextMessage
    {
        private string text;
        private string sender;
        private string sentiment= null;
        public TextMessage(string sender, string text)
        {
            this.sender = sender;
            this.text = text;
        }

        public string Sentiment
        {
            get { return sentiment;  }
            set { sentiment = value;  }
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