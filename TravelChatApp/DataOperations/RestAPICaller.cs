using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;

namespace TravelChatApp.DataOperations
{
    class RestAPICaller
    {

        private static string apiURL = "https://chatbotrestapi.conveyor.cloud/";
        private static string location;
        private static string getResponseAPIResult;

        public static string Location{ get { return location; } }
        public static string GetResponseAPIResult { get { return getResponseAPIResult; } }


        public static string CallApiGetParagraphs(string input)
        {
            RestClient client = new RestClient(apiURL + "/api/response/getparagraphs/" + input);
            var request = new RestRequest(Method.GET);

            IRestResponse responseAPI = client.Execute(request);
            return responseAPI.Content;
        }
        public static void CallApiGetResponse(string input, string constant)
        {
            string filters;
            RestClient client;

            if (ChatBotConversation.State == 10 || ChatBotConversation.State == 11 || ChatBotConversation.State == 12)
            {
                if (input.Split(":").Length > 1)
                {
                    filters = input.Split(":")[1];
                }
                else filters = "";
                client = new RestClient(apiURL + "/api/response/getresponse/" + location + " " + constant + " " + filters);
            }
            else
                client = new RestClient(apiURL + "/api/response/getresponse/" + location + " " + input);

            var request = new RestRequest(Method.GET);
            IRestResponse responseAPI = client.Execute(request);
            getResponseAPIResult = responseAPI.Content;

            if (getResponseAPIResult == null || getResponseAPIResult.Contains("null") || getResponseAPIResult.Contains("error") || getResponseAPIResult.Contains("Time-out"))
                ChatBotConversation.State = 6;
            else if (ChatBotConversation.State == 10 || ChatBotConversation.State == 11 || ChatBotConversation.State == 12)
                ChatBotConversation.State = 13;
            else
                ChatBotConversation.State = 5;


        }
        public static bool CallCheckLocationAPI(string input)
        {
            var client = new RestClient(apiURL + "/api/response/getchecklocation/" + input);
            var request = new RestRequest(Method.GET);

            IRestResponse responseAPI = client.Execute(request);
            string response = responseAPI.Content;

            if (response != null)
            {
                if (response.Contains("True"))
                {
                    location = input;
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}