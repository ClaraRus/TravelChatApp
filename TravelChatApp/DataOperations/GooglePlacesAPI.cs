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
        private static string urlFindPlace = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=";
        private static string urlPlaceDetais = "https://maps.googleapis.com/maps/api/place/details/json?place_id=";
        private static string APIKey = "AIzaSyCvjLMCo5UOJFQ086fslK1p-nsdpU-CBXo";
        public GooglePlacesAPI()
        {
            SetUri("https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input");
        }
        
        //it may take a while to retrive the data (read about async and Task)
        //function to create the api url like in the above example
        public static async Task<Place_Id> SearchPlaces(string input)
        {
            if(input!=null)
            {
                urlFindPlace += input + "&inputtype=textquery&fields=place_id&key=" + APIKey;
            }

            using (HttpResponseMessage response =  API.ApiClient.GetAsync(urlFindPlace).Result)
            {
                    if (response.IsSuccessStatusCode)
                    {
                        Place_Id place_id = await response.Content.ReadAsAsync<Place_Id>();
                        return place_id;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                
            }
        }

        public static async Task<Place> DetailsOfPlace(string input)
        {
            if (input != null)
            {
                urlPlaceDetais += input + "&fields=name,rating,reviews&key=" + APIKey;
            }
            
            using (HttpResponseMessage response =  API.ApiClient.GetAsync(urlPlaceDetais).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    Place place = await response.Content.ReadAsAsync<Place>();
                    if (place.Status.Equals("OK"))
                        return place;
                    else if(place.Status.Equals("OVER_QUERY_LIMIT"))
                    {
                        return place;

                    }
                    else throw new Exception("Place Error!");
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }
}