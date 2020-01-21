using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;
using TravelChatApp.Datastore;
using TravelChatApp.Datastore.FlightDetails;
using Newtonsoft.Json;
using TravelChatApp.Datastore.FlightDetails.DestinationDetails;

namespace TravelChatApp.DataOperations
{
    class SkyScannerAPI
    {
        private static string OutDate { get; set; }
        private static string InDate { get; set; }
        private static string ToDestination { get; set; }
        private static string FromDestination { get; set; }

        private static int state=0;

        private static Dictionary<string, string> locations = new Dictionary<string, string>();

        public static void RestValues()
        {
            OutDate = null;
            InDate = null;
            ToDestination = null;
            FromDestination = null;
            state = 0;
            locations = new Dictionary<string, string>();
        }
        public static string GetResponse(string input, Dictionary<List<string>, string> keywords)
        {
            string response="";
            input = input.ToLower();

            if (FromDestination != null && ToDestination != null && InDate != null && OutDate != null)
            {
                response = GetBestPrice(CallAPI());
            }
            else
            {
                if (FromDestination == null || ToDestination == null)
                    response = Locations(input, keywords);

                if (FromDestination!=null && ToDestination!=null) //we have both destinations
                {
                    DateTime dDate;
                    if (DateTime.TryParse(input, out dDate))
                    {
                        if (state == 3)
                            InDate = input;
                        else if (state == 4)
                            OutDate = input;

                    }

                   
                    if (InDate == null)
                    {

                        response = "When do you want to leave to " + locations[ToDestination] + " ?\n"
                            + "Format date: yyyy-mm-dd";
                        state = 3;
                    }
                    else
                    if (OutDate == null)
                    {
                        response = "When do you want to return to " + locations[FromDestination] + " ?\n"
                            + "Format date: yyyy-mm-dd";
                        state = 4;
                    }

                }

            }

            if (FromDestination != null && ToDestination != null && InDate != null && OutDate != null)
            {
                response = GetBestPrice(CallAPI());
            }
            return response;
        }

        private static string  FormatDestination(string destination)
        {
            var client = new RestClient("https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/autosuggest/v1.0/UK/EUR/en-GB/?query="+destination);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "69bd57d00amsh33b68481e90c518p11f660jsnfd640067af05");
            IRestResponse response = client.Execute(request);

            DestinationResult destinationResult = JsonConvert.DeserializeObject<DestinationResult>(response.Content);

            if(destinationResult.Places!=null)
                if (destinationResult.Places.Count != 0)
                    return destinationResult.Places[0].PlaceId.Replace("-sky", "");
            return destination;

        }
        private static string GetDestination(string input, string typeOfDest, Dictionary<List<string>, string> keywords)
        {
            string destination = null;
            string formattedDestination = null;
            int count = 0;
                
                    foreach (List<string> key in keywords.Keys)
                    {
                        foreach (string type in key)
                        {
                            if (type.Contains("City"))
                            {
                                if (input.Contains(typeOfDest))
                                {
                                    destination = input.Split(typeOfDest)[1].Split(" ")[1];
                                    formattedDestination = FormatDestination(destination);
                                    locations.Add(formattedDestination, destination);
                                    return formattedDestination;
                                }
                                else
                                {
                                    if (!locations.Keys.Contains(count.ToString()) && !locations.Values.Contains(keywords[key]))
                                    {
                                        locations.Add(count.ToString(), keywords[key]);
                                        count++;
                                    }
                                }
                                
                               
                            }
                        }
                    }
                 
            return formattedDestination;
        }
        private static string Locations(string input, Dictionary<List<string>, string> keywords)
        {

            
            if(FromDestination==null)
                FromDestination = GetDestination(input, "from", keywords);

            if(ToDestination==null)
                ToDestination = GetDestination(input, "to", keywords);


            if (FromDestination!=null && ToDestination!=null)
                return null;
            else if(locations.Count == 2)
            {
                if(ToDestination!=null && FromDestination==null)
                {
                    FromDestination = FormatDestination(locations["0"]);
                    locations.Add(FromDestination, locations["0"]);
                    locations.Remove("0");
    
                }
                else
                if (ToDestination == null && FromDestination != null)
                {
                    ToDestination = FormatDestination(locations["0"]);
                    locations.Add(ToDestination, locations["0"]);
                    locations.Remove("0");

                }
                else
                {
                    FromDestination = FormatDestination(locations["0"]);
                    locations.Add(FromDestination, locations["0"]);
                    locations.Remove("0");
                    ToDestination = FormatDestination(locations["1"]);
                    locations.Add(ToDestination, locations["0"]);
                    locations.Remove("1");

                    return null;
                }

           
            }
            else if(locations.Count==1)
            {
                    if ((ToDestination != null && FromDestination == null) || state ==1)
                    {
                        FromDestination = FormatDestination(locations["0"]);
                        locations.Add(FromDestination, locations["0"]);
                        locations.Remove("0");
                        state = 2;
                        return "Where do you want to fly to?";
                    }
                    else
                if (ToDestination == null && FromDestination != null)
                    {
                        ToDestination = FormatDestination(locations["0"]);
                        state = 1;
                        locations.Add(FromDestination, locations["0"]);
                        locations.Remove("0");
                        return "Where do you want to fly from?";
                    }
               
            }
            else if(locations.Count==0)
            {
                state = 1;
                return "Where do you want to fly from?";
            }

            if (FromDestination != null && ToDestination != null)
                return null;

            return "I do not understand...";
        }

        private static string GetBestPrice(FlightResult flightResult)
        {
            double min = Double.MaxValue;
            Quotes minQuotes=null;
            string response;

            foreach (Quotes quotes in flightResult.Quotes)
            {
                if (Double.Parse(quotes.MinPrice) < min)
                {
                    min = Double.Parse(quotes.MinPrice);
                    minQuotes = quotes;
                }
            }

            response = "Best price: " + min+" EUR. ";
            response += "With company ";
            Carriers carriers;
            foreach (string carrierIds in minQuotes.InboundLeg.CarrierIds)
            {
                carriers = flightResult.Carriers.Find( x=> x.CarrierId.Equals(carrierIds));
                response += carriers.Name + ",";
                
            }

            response = response.Remove(response.Length - 1);
            response += " and return with ";

            foreach (string carrierIds in minQuotes.OutboundLeg.CarrierIds)
            {
                carriers = flightResult.Carriers.Find(x => x.CarrierId.Equals(carrierIds));
                response += carriers.Name + ",";

            }

            response = response.Remove(response.Length - 1);
            response += ".";

            ChatBotConversation.State = 0;

            return response;
        }
        private static FlightResult CallAPI()
        {
            var client = new RestClient("https://skyscanner-skyscanner-flight-search-v1.p.rapidapi.com/apiservices/browseroutes/v1.0/RO/"+ "EUR" +"/ro/" + FromDestination +"/"+ToDestination+"/"+ InDate + "/"+ OutDate);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "skyscanner-skyscanner-flight-search-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "69bd57d00amsh33b68481e90c518p11f660jsnfd640067af05");
            IRestResponse response = client.Execute(request);

            FlightResult flightResult = JsonConvert.DeserializeObject<FlightResult>(response.Content);

            return flightResult;
           
        }
    }
}