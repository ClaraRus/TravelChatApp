using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using TravelChatApp.Datastore;
using static Android.Support.V7.Widget.RecyclerView;

namespace TravelChatApp
{

    class SuggestionRecycler : RecyclerView.Adapter
    {
        private static int VIEW_TYPE_ANSWER = 1;
        private static int VIEW_TYPE_CHECKBOX = 2;
        private static int VIEW_TYPE_TEXTBOX = 3;
        private static int VIEW_TYPE_READY = 4;

        private MainActivity mainActivity;

        public string[] suggestions;

        public bool isButton;
        private LayoutInflater inflater;

        public SuggestionRecycler(MainActivity mainActivity, string [] suggestions)
        {
            inflater = LayoutInflater.From(mainActivity);
            this.mainActivity = mainActivity;
            this.suggestions = suggestions;
        }

        public string [] Answer
        {
            set { suggestions = value; }
            get { return suggestions; }
        }
        public int Count
        {
            get { return suggestions.Length; }
        }

        public override int ItemCount
        {
            get { return suggestions.Length; }
        }



        public override long GetItemId(int position)
        {
            return position;
        }
        // Determines the appropriate ViewType according to the sender of the message.
        public override int GetItemViewType(int position)
        {
            string suggestion = suggestions[position];
            
            
                bool value = suggestion.Equals("Type");
                bool val2 = suggestion.Equals("Ready");

                if (isButton)
                    return VIEW_TYPE_ANSWER;
                else if (suggestion.Equals("Ready"))
                    return VIEW_TYPE_READY;
                else if (suggestion.Equals("Type"))
                    return VIEW_TYPE_TEXTBOX;
                else return VIEW_TYPE_CHECKBOX;
            
        }

        // Inflates the appropriate layout according to the ViewType.

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            View view;
            //LayoutInflater inflater = (LayoutInflater)mainActivity.BaseContext.GetSystemService(Context.LayoutInflaterService);
            if (viewType == VIEW_TYPE_ANSWER)
            {
                view = inflater.Inflate(Resource.Layout.ChooseButton, null);
                return new SuggestionHolder(view, mainActivity);
            }
            if(viewType == VIEW_TYPE_CHECKBOX)
            {
                view = inflater.Inflate(Resource.Layout.CheckBox, null);
                return new CheckBoxHolder(view, mainActivity);
            }
            if (viewType == VIEW_TYPE_TEXTBOX)
            {
                view = inflater.Inflate(Resource.Layout.TextBox, null);
                return new TextBoxHolder(view, mainActivity);
            }
            if (viewType == VIEW_TYPE_READY)
            {
                view = inflater.Inflate(Resource.Layout.ReadyButton, null);
                return new ReadyHolder(view, mainActivity);
            }


            return null;
        }

        // Passes the message object to a ViewHolder so that the contents can be bound to UI.

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            string suggestion = suggestions[position];

            switch (holder.ItemViewType)
            {
                case 1:
                    ((SuggestionHolder)holder).bind(suggestion);
                    break;
                case 2:
                    ((CheckBoxHolder)holder).bind(suggestion);
                    break;
                case 3:
                    ((TextBoxHolder)holder).bind(suggestion);
                    break;
                case 4:
                    ((ReadyHolder)holder).bind(suggestion);
                    break;




            }
        }


        private class ReadyHolder : RecyclerView.ViewHolder
        {
            Button button;
            MainActivity mainActivity;

            public ReadyHolder(View itemView, MainActivity mainActivity) : base(itemView)
            {
                button = (Button)itemView.FindViewById(Resource.Id.readyButton);
                this.mainActivity = mainActivity;
            }

            public void bind(string suggestion)
            {
                
                button.Text = suggestion;
                button.Click += delegate {
                    mainActivity.FilterSend();
                   // button.Text = string.Format("{0} clicks!", count++);
                };
            }
        }

        private class TextBoxHolder : RecyclerView.ViewHolder
        {
            EditText textBox;
            MainActivity mainActivity;
            public TextBoxHolder(View itemView, MainActivity mainActivity) : base(itemView)
            {
                textBox = (EditText)itemView.FindViewById(Resource.Id.editText1);
                this.mainActivity = mainActivity;
            }

            public void bind(string suggestion)
            {
               
                //textBox.Text = suggestion;
                /*button.Click += delegate {
                    mainActivity.ChoiceSend(button.Text);
                    //button.Text = string.Format("{0} clicks!", count++);
                };*/
            }
        }
        private class CheckBoxHolder: RecyclerView.ViewHolder
        {
            CheckBox checkBox;
            MainActivity mainActivity;
            public CheckBoxHolder(View itemView, MainActivity mainActivity) : base(itemView)
            {
                checkBox = (CheckBox)itemView.FindViewById(Resource.Id.checkbox);
                this.mainActivity = mainActivity;
            }

            public void bind(string suggestion)
            {
                
                checkBox.Text = suggestion;
                bool val = checkBox.Checked;
                checkBox.CheckedChange += delegate { mainActivity.UpdateFilters(checkBox.Checked, checkBox.Text); };

                /*button.Click += delegate {
                    mainActivity.ChoiceSend(button.Text);
                    //button.Text = string.Format("{0} clicks!", count++);
                };*/
            }
        }
        private class SuggestionHolder : RecyclerView.ViewHolder
        {
            Button button;
            MainActivity mainActivity;

            public SuggestionHolder(View itemView, MainActivity mainActivity) : base(itemView)
            {
                button = (Button)itemView.FindViewById(Resource.Id.answer);
                this.mainActivity = mainActivity;
            }

            public void bind(string suggestion)
            {
                
                button.Text = suggestion;
                button.Click += delegate {
                    mainActivity.ChoiceSend(button.Text);
                    //button.Text = string.Format("{0} clicks!", count++);
                };
           }
        }

    }
}