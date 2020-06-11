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
using RestSharp;
using RestSharp.Extensions;

namespace TravelChatApp.Datastore
{
    public class GooglePlacesAPI
    {
        private static string urlFindPlace = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=";
        private static string urlPlaceDetais = "https://maps.googleapis.com/maps/api/place/details/json?place_id=";
        private static string APIKey = "AIzaSyCvjLMCo5UOJFQ086fslK1p-nsdpU-CBXo";

        public GooglePlacesAPI()
        {
        }

        public static Place_Id SearchPlaces(string input)
        {
            if (input != null)
            {
                urlFindPlace += input + "&inputtype=textquery&fields=place_id&key=" + APIKey;
                var restClient = new RestClient(urlFindPlace);
                var request = new RestRequest(Method.GET);

                var responseAPI = restClient.Execute<Place_Id>(request);
                if(responseAPI.IsSuccessful)
                {
                    Place_Id place_id = responseAPI.Data;
                    return place_id;
                }
                else throw new Exception(responseAPI.StatusCode.ToString());
            }
            return null;
        }

        public static Place DetailsOfPlace(string input)
        {
            if (input != null)
            {
                urlPlaceDetais += input + "&fields=name,rating,reviews&key=" + APIKey;
                var restClient = new RestClient(urlPlaceDetais);
                var request = new RestRequest(Method.GET);

                var responseAPI = restClient.Execute<Place>(request);
                if (responseAPI.IsSuccessful)
                {
                    Place place = responseAPI.Data;
                    if (place.Status.Equals("OK"))
                        return place;
                    else if (place.Status.Equals("OVER_QUERY_LIMIT"))
                    {
                        return place;

                    }
                    else throw new Exception("Place Error!");
                }
                else throw new Exception(responseAPI.StatusCode.ToString());

            }
            return null;
        }

        public static Place GetPlaceDetails(string location)
        {
            var place_id = SearchPlaces(location);
            if (place_id.status.Equals("OK"))
            {
                string placeId = "";
                Place place;

                foreach (Candidate c in place_id.candidates)
                    placeId = c.place_id;

                if (!placeId.Equals(""))
                {
                    place = DetailsOfPlace(placeId);
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
       



    }
}