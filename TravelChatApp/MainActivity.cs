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

namespace TravelChatApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        
        private  MessageRecycler adapter;
        private  List<TextMessage> textMessages = new List<TextMessage>();
        //private ListView chatList;
        RecyclerView chatList;
        private EditText inputMessage;
        private FloatingActionButton fab;
        private  string input;
        private string response;
        private string sentimentResponse;
        public static AssetManager assets;
        private ChatBotConversation conversation = new ChatBotConversation();

        public static Context context;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            context = this;

            adapter = new MessageRecycler(this, textMessages);

            assets = this.Assets;

            inputMessage = FindViewById<EditText>(Resource.Id.input);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            chatList = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            chatList.SetLayoutManager(new LinearLayoutManager(this));
            chatList.SetAdapter(adapter);

            DisplayChatMessage();

            API.InitializeClient();

            fab.Click += delegate { SendMethod(); };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void DisplayChatMessage()
        {
            adapter.TextMessages = textMessages;
            chatList.SetAdapter(adapter);
            adapter.NotifyItemInserted(textMessages.Count - 1);
            if(!textMessages[textMessages.Count-1].Sender.Equals("Travis_Load"))
                chatList.SmoothScrollToPosition(textMessages.Count - 1);



        }
        public void SendMethod()
        {
            input = inputMessage.Text;
            textMessages.Add(new TextMessage("User", input));
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

            response = await conversation.GetResponse(input);
            //response = "It is very nice";
            if(ChatBotConversation.State == 2)
                sentimentResponse = Paralleldots.GetSentiment(response);
            e.Result = "Done";

       }

       private void  SendResponse()
       {
            TextMessage reply=null;

            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(backgroundWorker_DoWorkAsync);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            bg.WorkerReportsProgress = true;
            bg.RunWorkerAsync();

            reply = new TextMessage("Travis_Load", "");
            textMessages.Add(reply);
            RunOnUiThread(new Runnable(DisplayChatMessage));

            while (bg.IsBusy);

            reply.Text = response;
            reply.Sender = "Travis";
            reply.Sentiment = sentimentResponse;

            RunOnUiThread(new Runnable(DisplayChatMessage));

        }



    }
}