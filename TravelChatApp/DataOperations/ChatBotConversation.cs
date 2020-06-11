using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParallelDots;
using RestSharp;
using TravelChatApp.Datastore;
using Xamarin.Forms.Internals;

namespace TravelChatApp.DataOperations
{
    class ChatBotConversation 
    {        
        private static Answer answer= new Answer("Travis","");
        private static string[] suggestions;
        public static int State { get; set; }
        private static string typeInput;
        public static Context context;
        public ChatBotConversation()
        {
           
        }
        public static Answer StartConversation(string input)
        {
            input = input.ToLower();
            switch(State)
            {
                case 0:
                    if (input.Equals(Constants.YES.ToLower()))
                        State = 1;
                    else if (input.Equals(Constants.NO.ToLower()))
                        State = 2;
                    return GetResponse(input);
                case 1:
                    if (RestAPICaller.CallCheckLocationAPI(input))
                        State = 3;
                    else State = 4;
                    return GetResponse(input);
                case 2: State = 0; return GetResponse(input);
                case 3:
                    if (input.Equals(Constants.OTHER.ToLower()))
                        State = 8;
                    else if (input.Equals(Constants.ACTIVITIES.ToLower()))
                        State = 10;
                    else if (input.Equals(Constants.ACCOMODATION.ToLower()))
                        State = 12;
                    else if (input.Equals(Constants.RESTAURANTS.ToLower()))
                        State = 11;   
                    return GetResponse(input);
                case 4:
                    if (RestAPICaller.CallCheckLocationAPI(input))
                        State = 3;
                    else if (input.Contains(Constants.NO.ToLower()))
                        State = 7;
                    else
                        State = 4; 
                    return GetResponse(input);
                case 5: State = 9; return GetResponse(input);
                case 6: State = 9; return GetResponse(input);
                case 7: State = 0; return GetResponse(input);
                case 8:
                    RestAPICaller.CallApiGetResponse(input, Constants.OTHER);
                    return GetResponse(input);
                case 9:
                    if (input.Equals(Constants.YES.ToLower()))
                        State = 3;
                    else if (input.Equals(Constants.NO.ToLower()))
                        State = 7;
                    return GetResponse(input);
                case 10:
                    typeInput = Constants.ACTIVITIES;
                    RestAPICaller.CallApiGetResponse(input, Constants.ACTIVITIES);
                    return GetResponse(input);
                case 11:
                    typeInput = Constants.RESTAURANTS;
                    RestAPICaller.CallApiGetResponse(input, Constants.RESTAURANTS);
                    return GetResponse(input);
                case 12:
                    typeInput = Constants.ACCOMODATION;
                    RestAPICaller.CallApiGetResponse(input, Constants.ACCOMODATION);
                    return GetResponse(input);
                case 13:
                    if(input.Equals(Constants.NOTHING.ToLower()))
                        State = 9;
                    else
                        StartActivity(PlaceInformation.GetPlaceInformation(input, RestAPICaller.Location,typeInput));
                    
                    return GetResponse(input);
            }
            return GetResponse(input);
        }

        private static Answer GetResponse(string input)
        {
            answer = new Answer("Travis", "");
            if(State == 0 )
            {
                answer.Text = "Can I help you ?";
                suggestions = new string[2]{Constants.YES, Constants.NO};
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 1)
            {
                answer.Text = "Where do you want to travel to ?";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 2)
            {
                answer.Text = "Bye";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 4)
            {
                answer.Text = "Hmm... Could you repeat ?";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 3)
            {
                answer.Text = "What would you like to know ?";
                //suggestions = new string[] { Constants.ACCOMODATION,Constants.RESTAURANTS, Constants.ACTIVITIES, Constants.OTHER};
                suggestions = new string[] { Constants.ACCOMODATION, Constants.RESTAURANTS, Constants.ACTIVITIES};
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 5)
            {
                answer.Text = RestAPICaller.GetResponseAPIResult;
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 6)
            {
                answer.Text = "Sorry... I did not found anything.";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 7)
            {
                answer.Text = "Ok. I am here when you need :)";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 8)
            {
                answer.Text = "Tell me :)";
                suggestions = new string[0];
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 9)
            {
                answer.Text = "Would you like to know something else ?";
                suggestions = new string[2] { Constants.YES, Constants.NO};
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = true;
            }
            else if(State == 10)
            { 
                answer.Text = "What are your preferences ?";
                suggestions = new string[] { "night", "outdoor", "kids", "free", "Type", "Ready" };
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = false;
            }
            else if (State == 11)
            {
                answer.Text = "What are your preferences ?";
                suggestions = new string[] { "cheap", "fun", "romantic", "interesting", "Type", "Ready" };
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = false;
            }
            else if (State == 12)
            {
                answer.Text = "What are your preferences ?";
                suggestions = new string[] { "hotel", "hostel", "apartment", "romantic", "cheap", "Type", "Ready" };
                answer.Suggestion.Text = suggestions;
                answer.Suggestion.IsButton = false;
            }
            else if(State == 13)
            {
                answer.Text = "What would you like to do?";
                answer.Suggestion.IsButton = true;
                suggestions = TextProcessing.DeserializeActivitiesAPI(RestAPICaller.GetResponseAPIResult);
                Array.Resize(ref suggestions, suggestions.Length + 1);
                suggestions[suggestions.Length - 1] = Constants.NOTHING;
                answer.Suggestion.Text = suggestions;
            }
            

            return answer;
        }

        private static void StartActivity(PlaceInformation placeInformation)
        {
            Intent intent = new Intent(context, typeof(PlaceInformationActivity));
            intent.PutExtra("placeInformation", JsonConvert.SerializeObject(placeInformation));

            context.StartActivity(intent);
        }




    }
}