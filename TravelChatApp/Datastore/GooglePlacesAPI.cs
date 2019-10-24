using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TravelChatApp.Datastore
{
    public class GooglePlacesAPI : API
    {
        public GooglePlacesAPI()
        {
            SetUri("https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input");
        }
        //example:
        //https://maps.googleapis.com/maps/api/place/findplacefromtext/json?
        //input=Museum%20of%20Contemporary%20Art%20Australia&inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry&key=YOUR_KEY
        //it may take a while to retrive the data (read about async and Task)
        //function to create the api url like in the above example
        public async Task SearchPlaces(string input)
        {
            string url = "";
            if(input!=null)
            {
                url += input;
            }
            //A NEW REQUEST and wait for response
            using (HttpResponseMessage response = await API.ApiClient.GetAsync(url))
            {
                if(response.IsSuccessStatusCode)
                {

                }
            }
        }
    }
}