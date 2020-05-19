using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;

namespace TravelChatApp.DataOperations
{


    class InputAnalyzer
    {

        private static string apiKey = "30b5519e6d3285699abf92762bdb5664 ";
        public InputAnalyzer()
        {

        }

        private static string TopicExtractionAPI(string word)
        {
            try
            {
                var client = new RestClient("https://api.meaningcloud.com/topics-2.0");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", "key=" + apiKey + "&lang=en&txt=" + word + "&tt=a", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        private static List<string> ExtractTypes(string apiResponse)
        {
            List<string> types = new List<string>();
            MatchCollection matches = Regex.Matches(apiResponse, "type\":\"[a-zA-Z>]*");
            string type;
            foreach (Match m in matches)
            {
                Console.WriteLine(m.Value);
                if (m.Value.Contains("Top>"))
                {
                    type = Regex.Split(m.Value, "type\":\"Top>")[1];
                    if (!types.Contains(type) && !type.Equals(""))
                        types.Add(type);
                }
            }
            //"type":"Top>Location>GeoPoliticalEntity>Country"}
            return types;
        }
        public static void GetCategorizedWords(string input, Dictionary<List<string>, string> categorizedWords)
        {

            List<string> tokenizedInput = input.Split(' ').ToList<string>();
            List<string> types = new List<string>();
            foreach (string word in tokenizedInput)
            {
                types = ExtractTypes(TopicExtractionAPI(word));
                if (types.Count != 0)
                    categorizedWords.Add(types, word);
            }


        }

        public static bool CheckLocation(string input)
        {
            List<string> types = new List<string>();
            types = ExtractTypes(TopicExtractionAPI(input));
            if (types.Count != 0)
                foreach (string type in types)
                {
                    if (type.Contains("City") || type.Contains("Country"))
                    {
                        return true;
                    }
                    return false;
                }
            return false;
        }
    }
}