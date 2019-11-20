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
using Java.Lang;
using TravelChatApp.Datastore;
using Object = Java.Lang.Object;

namespace TravelChatApp
{
    class ListViewAdapter : BaseAdapter
    {
        private MainActivity mainActivity;
        private List<TextMessage> textMessages;

        public ListViewAdapter(MainActivity mainActivity, List<TextMessage> textMessages)
        {
            this.mainActivity = mainActivity;
            this.textMessages = textMessages;
            this.textMessages.Add(new TextMessage("chatBot", "Hello, can I help you?"));
        }

        public List<TextMessage> TextMessages
        {
            set { textMessages = value; }
            get { return textMessages; }
        }
        public override int Count
        {
            get { return textMessages.Count; }
        }

        public override Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)mainActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            View ItemView = inflater.Inflate(Resource.Layout.ListMessagesLayout, null);
            TextView message_user, message_content;
            message_user = ItemView.FindViewById<TextView>(Resource.Id.message_user);
            message_content = ItemView.FindViewById<TextView>(Resource.Id.message_text);

            message_user.Text = textMessages[position].Sender;
            message_content.Text = textMessages[position].Text;

            return ItemView;
        }
    }
}