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
            if (input != null)
            {
                urlFindPlace += input + "&inputtype=textquery&fields=place_id&key=" + APIKey;
            }

            using (HttpResponseMessage response = API.ApiClient.GetAsync(urlFindPlace).Result)
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

            using (HttpResponseMessage response = API.ApiClient.GetAsync(urlPlaceDetais).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    Place place = await response.Content.ReadAsAsync<Place>();
                    if (place.Status.Equals("OK"))
                        return place;
                    else if (place.Status.Equals("OVER_QUERY_LIMIT"))
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

        public static async Task<Place> GetPlaceDetails(string location)
        {
            var place_id = await GooglePlacesAPI.SearchPlaces(location);

            if (place_id.status.Equals("OK"))
            {
                string placeId = "";
                Place place;

                foreach (Candidate c in place_id.candidates)
                    placeId = c.place_id;

                if (!placeId.Equals(""))
                {
                    place = await GooglePlacesAPI.DetailsOfPlace(placeId);
                    if (place.Status.Equals("OK"))
                        return place;
                    else if (place.Status.Equals("OVER_QUERY_LIMIT"))
                    {
                        return null;
                    }
                }
            }
            else if (place_id.status.Equals("OVER_QUERY_LIMIT"))
            {
                return null; ;
            }

            return null;
        }
        public static async Task<List<Review>> GetReviews(string location)
        {
            var place_id = await GooglePlacesAPI.SearchPlaces(location);

            if (place_id.status.Equals("OK"))
            {
                string placeId = "";
                Place place;

                foreach (Candidate c in place_id.candidates)
                    placeId = c.place_id;

                if (!placeId.Equals(""))
                {
                    place = await GooglePlacesAPI.DetailsOfPlace(placeId);
                    if (place.Status.Equals("OK"))
                        return place.Result.Reviews;
                    else if (place.Status.Equals("OVER_QUERY_LIMIT"))
                    {
                        Review response = new Review();
                        response.Text = "No more queries for today... Talk to you tomorrow :)";
                        List<Review> reviews = new List<Review>();
                        reviews.Add(response);
                        return reviews;
                    }
                }
            }
            else if (place_id.status.Equals("OVER_QUERY_LIMIT"))
            {
                Review response = new Review();
                response.Text = "No more queries for today... Talk to you tomorrow :)";
                List<Review> reviews = new List<Review>();
                reviews.Add(response);
                return reviews;
            }

            return null;
        }



    }
}