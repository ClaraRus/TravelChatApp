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
        private static string url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=";
        private static string APIKey = "AIzaSyDtsHEtgx_lpQ4CgGJJojsW_jxBZn4dr8A";
        public GooglePlacesAPI()
        {
            SetUri("https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input");
        }
        //example:
        //https://maps.googleapis.com/maps/api/place/findplacefromtext/json?
        //input=Museum%20of%20Contemporary%20Art%20Australia&inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry&key=YOUR_KEY
        //it may take a while to retrive the data (read about async and Task)
        //function to create the api url like in the above example
        public static async Task<Place> SearchPlaces(string input)
        {
            if(input!=null)
            {
                url += input + "&inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry&key=" + APIKey;
                //url = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=Museum%20of%20Contemporary%20Art%20Australia&inputtype=textquery&fields=photos,formatted_address,name,rating,opening_hours,geometry&key=" + APIKey;
            }
            //A NEW REQUEST and wait for response

            Console.WriteLine(url);
                using (HttpResponseMessage response = await API.ApiClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                        Place place = await response.Content.ReadAsAsync<Place>();


                        return place;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                
            }
        }

        public async Task DetailsOfPlace(string input)
        {
            string url = "";
            if (input != null)
            {
                url += input;
            }
            //A NEW REQUEST and wait for response
            using (HttpResponseMessage response = await API.ApiClient.GetAsync(url))
            {
                //de aici luam rewies
                if (response.IsSuccessStatusCode)
                {

                }
            }
        }


    }
}