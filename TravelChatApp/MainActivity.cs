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

namespace TravelChatApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        
        private ListViewAdapter adapter;
        private List<TextMessage> textMessages = new List<TextMessage>();
        private ListView chatList;
        private EditText inputMessage;
        private FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            adapter = new ListViewAdapter(this, textMessages);
           

            inputMessage = FindViewById<EditText>(Resource.Id.input);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            chatList = FindViewById<ListView>(Resource.Id.list_of_messages);

            fab.Click += delegate { SendMethod(); };
            API.InitializeClient();
            DisplayChatMessage();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void PostMessage()
        {
            
        }

        private void DisplayChatMessage()
        {
            adapter.TextMessages = textMessages;
            chatList.Adapter = adapter;
            
        }
        public async void SendMethod()
        {
            textMessages.Add(new TextMessage("user", inputMessage.Text));
            DisplayChatMessage();

            string input=  "Museum%20of%20Contemporary%20Art%20Australia" ;
            
            inputMessage.Text = "";

            await SearchPlaces(input);
            DisplayChatMessage();
            //trebe trimis mesajul ca inpu la API
            //trebe retinut raspunsul de la API 
            //trebe afisat raspunsul de la API

            //de citit citeste, dar nu serializeaza datele in Place 
        }

        private async Task SearchPlaces(string input)
        {
            var place = await GooglePlacesAPI.SearchPlaces(input);
            foreach(Candidate c in place.Candidates)
            textMessages.Add(new TextMessage("chatBot", c.Rating));
        }
        
    }
}