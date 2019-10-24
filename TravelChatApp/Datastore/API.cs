using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TravelChatApp.Datastore
{
    public abstract class API
    {


        //static because we want to open it once per application instance 
        public static HttpClient ApiClient { get; set; }
        public static string apiUri;

        //set up API Client
        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri(apiUri);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            // header that gets json -> we can pare it into models (we just want data structured as json)
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void SetUri(string apiUri)
        {
            API.apiUri = apiUri;
        }
    }
}