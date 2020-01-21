using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Graphics.Drawable;
using Android.Support.V7.Content.Res;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Felipecsl.GifImageViewLibrary;
using Java.Lang;
using TravelChatApp.Datastore;
using static Android.Support.V7.Widget.RecyclerView;
using Object = Java.Lang.Object;

namespace TravelChatApp
{
    class MessageRecycler : RecyclerView.Adapter
    {
        private MainActivity mainActivity;
        private List<TextMessage> textMessages;
        private static  int VIEW_TYPE_MESSAGE_SENT = 1;
        private static  int VIEW_TYPE_MESSAGE_RECEIVED = 2;
        private static int VIEW_TYPE_LOAD_ANSWER = 3; 


        public MessageRecycler(MainActivity mainActivity, List<TextMessage> textMessages)
        {
            this.mainActivity = mainActivity;
            this.textMessages = textMessages;
            this.textMessages.Add(new TextMessage("Travis", "Hello, can I help you with an opinion on a place or search a flight?"));
        }

        public List<TextMessage> TextMessages
        {
            set { textMessages = value; }
            get { return textMessages; }
        }
        public  int Count
        {
            get { return textMessages.Count; }
        }

        public override int ItemCount
        {
            get { return textMessages.Count; }
        }

        

        public override long GetItemId(int position)
        {
            return position;
        }
        // Determines the appropriate ViewType according to the sender of the message.
        public override int GetItemViewType(int position)
        {
            TextMessage message = (TextMessage)textMessages[position];

             if (message.Sender.Equals("Travis"))
            {
                // If the current user is the sender of the message
                return VIEW_TYPE_MESSAGE_RECEIVED;
            }
            else if(message.Sender.Equals("User"))
            {
                // If some other user sent the message
                return VIEW_TYPE_MESSAGE_SENT;
            }
            else if(message.Sender.Equals("Travis_Load"))
            {
                return VIEW_TYPE_LOAD_ANSWER;
            }

            return 0;
        }

        // Inflates the appropriate layout according to the ViewType.

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            
            View view;
            LayoutInflater inflater = (LayoutInflater)mainActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            if (viewType == VIEW_TYPE_MESSAGE_SENT)
            {
                view = inflater.Inflate(Resource.Layout.ListMessagesUserLayout, null);
                return new SentMessageHolder(view);
            }
            else if (viewType == VIEW_TYPE_MESSAGE_RECEIVED)
            {
                view = inflater.Inflate(Resource.Layout.ListMessagesLayout, null);
                return new ReceivedMessageHolder(view);
            }
            else if (viewType == VIEW_TYPE_LOAD_ANSWER)
            {
                view = inflater.Inflate(Resource.Layout.MessageLoadingLayout, null);
                return new LoadAnswerMessageHolder(view);
            }

            return null;
        }

        // Passes the message object to a ViewHolder so that the contents can be bound to UI.

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TextMessage message = (TextMessage)textMessages[position];

            switch (holder.ItemViewType)
            {
                case 1:
                    ((SentMessageHolder)holder).bind(message);
                    break;
                case 2:
                    ((ReceivedMessageHolder)holder).bind(message);
                    break;
                case 3:
                    ((LoadAnswerMessageHolder)holder).bind();
                    break;

            }
         }

        
   
        private class SentMessageHolder : RecyclerView.ViewHolder
        {
            TextView messageText;

            public SentMessageHolder(View itemView) : base(itemView)
            {
                messageText = (TextView)itemView.FindViewById(Resource.Id.message_text);
            }

             public void bind(TextMessage message)
             {
               messageText.Text = message.Text;
             }
        }

        private class ReceivedMessageHolder : RecyclerView.ViewHolder
        {
            TextView messageText;

            public ReceivedMessageHolder(View itemView) : base(itemView) {
                messageText = (TextView)itemView.FindViewById(Resource.Id.message_text);
            }

            public void bind(TextMessage message) {

                messageText.Text = message.Text;

                switch(message.Sentiment)
                {
                    case "positive": messageText.SetBackgroundResource(Resource.Drawable.ChatBotMessagePositive); break;
                    case "negative": messageText.SetBackgroundResource(Resource.Drawable.ChatBotMessageNegative); break;
                    case "neutral": messageText.SetBackgroundResource(Resource.Drawable.ChatBotMessageNeutral); break;
                    default: messageText.SetBackgroundResource(Resource.Drawable.ChatBotMessage); break;
                }
                
            }
        }

        private class LoadAnswerMessageHolder : RecyclerView.ViewHolder
        {
            GifImageView image;


            public LoadAnswerMessageHolder(View itemView) : base(itemView)
            {
                image= itemView.FindViewById<GifImageView>(Resource.Id.gifImageView);
            }
            
            public void bind()
            {
                AssetManager assets = MainActivity.assets;
               
                Stream input= assets.Open("giphyPapper.gif");
                byte[] bytes = ConvertFileToByteArray(input);
                image.SetBytes(bytes);
               image.StartAnimation();
            }

            private byte[] ConvertFileToByteArray(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        ms.Write(buffer, 0 , read);

                    return ms.ToArray();
                }
            }


        }
    }
}