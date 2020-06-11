using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using TravelChatApp.Datastore;
using System.Collections.Generic;
using Android.Views;
using Android.Support.Design.Widget;
using System.Threading.Tasks;
using TravelChatApp.DataOperations;
using Android.Support.V7.Widget;
using System.Threading;
using System.Runtime.CompilerServices;
using Java.Lang;
using Thread = System.Threading.Thread;
using System.ComponentModel;
using Android.Content.Res;
using ParallelDots;
using Android.Content;
using Xamarin.Forms.Platform.Android.AppCompat;
using Toolbar = Android.Widget.Toolbar;

namespace TravelChatApp
{
    //[Activity(Label = "@string/action_bar", Theme = "@style/AppTheme", MainLauncher = true)]
    [Activity(MainLauncher = true)]
    
    public class MainActivity : AppCompatActivity
    {
        
        private  MessageRecycler adapter;

        private List<Answer> textMessages = new List<Answer>();

        //private ListView chatList;
        RecyclerView chatList;

        private EditText inputMessage;
        private FloatingActionButton fab;
        public static List<Button> buttons = new List<Button>();
        private static string input;
        private Answer response;
        
        public static AssetManager assets;
        public static Context context;
        protected override void OnCreate(Bundle savedInstanceState)
         {
            
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            context = this;

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.SetNavigationIcon(Resource.Drawable.airplane);
            toolbar.Title = "Travis :)";
            //Toolbar will now take on default actionbar characteristics
            this.SetActionBar(toolbar);
            //ActionBar.Title = "Travis :)";
            //ActionBar.SetIcon(Resource.Drawable.airplane);


            ChatBotConversation.context = context;

            adapter = new MessageRecycler(this, textMessages);

            assets = this.Assets;

            inputMessage = FindViewById<EditText>(Resource.Id.input);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            
            chatList = FindViewById<RecyclerView>(Resource.Id.recyclerView);
           
            chatList.SetLayoutManager(new GridLayoutManager(context, 1, LinearLayoutManager.Vertical, false));
            chatList.SetAdapter(adapter);


            DisplayChatMessage();

            
            fab.Click += delegate { SendMethod(); };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        private void DisplayChatMessage()
        {

            Answer lastMessage = textMessages[textMessages.Count - 1];
            adapter.TextMessages = textMessages;

            if(lastMessage.Sender.Equals("Travis"))
            {
                if (lastMessage.Suggestion.Text.Length > 0)
                {
                    fab.Visibility = ViewStates.Invisible;
                    inputMessage.Visibility = ViewStates.Invisible;
                }
                
            }
            else
            {
                fab.Visibility = ViewStates.Visible;
                inputMessage.Visibility = ViewStates.Visible;
            }

            chatList.SetAdapter(adapter);
            adapter.NotifyItemInserted(textMessages.Count - 1);
            if(!lastMessage.Sender.Equals("Travis_Load"))
                chatList.SmoothScrollToPosition(textMessages.Count - 1);

            if(ChatBotConversation.State==5 || ChatBotConversation.State==6)
            {
                input = "";
                Thread thr = new Thread(new ThreadStart(SendResponse));
                thr.Start();
            }
            

        }

        List<string> filters = new List<string>();
        public void UpdateFilters(bool isChecked, string text)
        {
            if (isChecked)
            {
                filters.Add(text);
            }
            else if(filters.Contains(text))
            {
                filters.Remove(text);
            }
        }
        public void FilterSend()
        {
            if (filters.Count == 0)
                input = "I do not have preferences";
            else 
            {
                input = "My preferences are: ";
                foreach(string filter in filters)
                {
                    input += filter + ", ";
                }
                input = input.Remove(input.Length-2);
            }
            textMessages[textMessages.Count - 1].Suggestion.Text = new string[0];
            textMessages.Add(new Answer("User", input));

            fab.Visibility = ViewStates.Visible;
            inputMessage.Visibility = ViewStates.Visible;

            filters = new List<string>();

            DisplayChatMessage();
            Thread thr = new Thread(new ThreadStart(SendResponse));
            thr.Start();

        }
        public void ChoiceSend(string choice)
        {
            if (choice != null)
            {
                input = choice;
                textMessages[textMessages.Count - 1].Suggestion.Text = new string[0];
                textMessages.Add(new Answer("User", input));

                fab.Visibility = ViewStates.Visible;
                inputMessage.Visibility = ViewStates.Visible;

                DisplayChatMessage();
                Thread thr = new Thread(new ThreadStart(SendResponse));
                thr.Start();
            }   
        }
        public void SendMethod()
        {
            input = inputMessage.Text;
            textMessages.Add(new Answer("User", input));
            DisplayChatMessage();
            inputMessage.Text = "";

            Thread thr = new Thread(new ThreadStart(SendResponse));
            thr.Start();
        }


        private void backgroundWorker_RunWorkerCompleted(
             object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.
                //response = e.Result.ToString();
            }

        }
        private async void backgroundWorker_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation
            // to the Result property of the DoWorkEventArgs
            // object. This is will be available to the 
            // RunWorkerCompleted eventhandler.

            response = ChatBotConversation.StartConversation(input);
          
            //response = "It is very nice";
            //if(ChatBotConversation.State == 2)
              //  sentimentResponse = Paralleldots.GetSentiment(response);
            e.Result = "Done";

       }

       private void  SendResponse()
       {
            Answer reply=null;

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(backgroundWorker_DoWorkAsync);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bg.WorkerReportsProgress = true;
            bg.RunWorkerAsync();

            reply = new Answer("Travis_Load", "");
            textMessages.Add(reply);
            RunOnUiThread(new Runnable(DisplayChatMessage));

            while (bg.IsBusy);


            reply.Sender =response.Sender;
            reply.Text = response.Text;
            reply.Suggestion = response.Suggestion;
            
            RunOnUiThread(new Runnable(DisplayChatMessage));
        }



    }
}