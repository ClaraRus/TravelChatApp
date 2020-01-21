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
using TravelChatApp.Datastore;

namespace TravelChatApp.DataOperations
{
    class ChatBotConversation 
    {
        private static Dictionary<List<string>, string> keywords;
        private static List<Review> reviews;
        private static string location;
        private static string response;

        public static int State { get; set; }
        public ChatBotConversation()
        {
            keywords = new Dictionary<List<string>, string>();
            reviews = new List<Review>();
        }
        public async Task<string> GetResponse(string input)
       {
            InputAnalyzer.GetCategorizedWords(input, keywords);

            input = input.ToLower();

            if (input.Equals("hello") || input.Equals("ok"))
            {
                response = "What would you like to know?Flight or opinion?";
                State = 0;
                keywords = new Dictionary<List<string>, string>();
                SkyScannerAPI.RestValues();
            }
            if (input.Contains("thank"))
            {
                response = "You are  welcome! What would you like to know? Flight or opinion?";
                State = 0;
                keywords = new Dictionary<List<string>, string>();
                SkyScannerAPI.RestValues();
            }
            else if (input.Contains("flight") || State == 1)
            {
                State = 1;
                response = SkyScannerAPI.GetResponse(input,keywords);
            }
            else if (input.Contains("opinion") || State == 2)
            {
                State = 2;
                if(input.Contains("opinion"))
                {
                    response = "What are you interested in?";
                }
                if (checkLocation())
                {
                    if (keywords.Count == 0)
                        response = "What aspects are you interested in?";
                    else
                    {
                        reviews = await GetReviews(location);
                        if (reviews != null)
                        {
                            if (reviews[0].Text.Contains("No more queries"))
                            {
                                response = reviews[0].Text;
                                keywords = new Dictionary<List<string>, string>();
                                State = 0;
                            }
                            else
                                response = ResponseAnalyzer.GetResponse(reviews, keywords);
                        }
                        else
                        {
                            response = "Sorry... I could not find any information.";
                            keywords = new Dictionary<List<string>, string>();
                            State = 0;
                        }

                        if (response.Equals(""))
                        {
                            response = "Sorry... I could not find any information.";
                            keywords = new Dictionary<List<string>, string>();
                            State = 0;
                        }
                    }
                }
                else if (keywords.Count != 0)
                {
                    response = "What location did you mean?";
                }

                if(response==null || response.Equals(""))
                    response = "I do not understand...";
            }
            return response;
       }

        private static bool checkLocation()
        {
            if (location == null)
            {
                foreach (List<string> key in keywords.Keys)
                {
                    foreach (string type in key)
                    {
                        if (type.Contains("City") || type.Contains("Country"))
                        {
                            location = keywords[key];
                            keywords.Remove(key);
                            return true;
                        }
                    }
                }


                return false;
            }

            return true;
        }


        private static async Task<List<Review>> GetReviews(string location)
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
            else if(place_id.status.Equals("OVER_QUERY_LIMIT"))
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