using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TravelChatApp.Datastore;

namespace TravelChatApp
{
    [Activity(MainLauncher = false)]
    class PlaceInformationActivity : AppCompatActivity
    {

        PlaceInformation placeInformation;

        Button btn;

        TextView textTitle;
        TextView textContent;
        TextView textRating;
        TextView textSentiment;

        ImageView heartImage;

        
        
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ResultLayout);

            
            placeInformation = JsonConvert.DeserializeObject<PlaceInformation>(Intent.GetStringExtra("placeInformation"));

            btn = FindViewById<Button>(Resource.Id.direction);
            btn.Click += delegate { GetDirections(); };

            textTitle = FindViewById<TextView>(Resource.Id.textTitle);
            textTitle.Text = placeInformation.Title;

            textContent = FindViewById<TextView>(Resource.Id.textContent);
            textContent.Text = placeInformation.Content;


            textRating = FindViewById<TextView>(Resource.Id.ratingText);
            textRating.Text = placeInformation.Rating.ToString();

            textSentiment = FindViewById<TextView>(Resource.Id.sentimentText);
            textSentiment.Text = GetTextSentiment();

            heartImage = FindViewById<ImageView>(Resource.Id.heart);
            heartImage.SetImageResource(GetHeartImagePath());

        }

       private int GetHeartImagePath()
       {
            string sentimentText = placeInformation.Sentiment;
            if (sentimentText.Equals(Constants.POSITIVE))
                return Resource.Drawable.Heart;
            else if (sentimentText.Equals(Constants.NEUTRAL))
                return Resource.Drawable.HeartNeutral;
            else if (sentimentText.Equals(Constants.NEGATIVE))
                return Resource.Drawable.HeartNegative;
            else return Resource.Drawable.Heart;


        }
        private string GetTextSentiment()
        {
            string sentimentText = placeInformation.Sentiment;
            if (sentimentText.Equals(Constants.POSITIVE))
                return "You will love it!";
            else if (sentimentText.Equals(Constants.NEUTRAL))
                return "You should give it a try!";
            else if (sentimentText.Equals(Constants.NEGATIVE))
                return "You can skip it...";

            return "";
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void GetDirections()
        {
           

            string location=placeInformation.Location.Replace(" ","+");
            string direction= placeInformation.Title.Replace(" ", "+");

            
                var gmmIntentUri = Android.Net.Uri.Parse("google.navigation:q="+ direction + ",+"+ location + "&mode=w");
                Intent mapIntent = new Intent(Intent.ActionView, gmmIntentUri);
                mapIntent.SetPackage("com.google.android.apps.maps");
                StartActivity(mapIntent);
            
        }

     

        
    }
}