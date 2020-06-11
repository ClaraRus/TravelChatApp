using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using ParallelDots;
using TravelChatApp.DataOperations;

namespace TravelChatApp.Datastore
{
	class PlaceInformation
	{
		public string Location { set; get; }
		public string Title { set; get; }
		public string Content { set; get; }
		public double Rating { set; get; }
		public string Sentiment { set; get; }

        public static PlaceInformation GetPlaceInformation(string input, string location, string typeInput)
        {
            PlaceInformation placeInformation = new PlaceInformation();
            placeInformation.Location = location.Trim();

            placeInformation.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());

            string content = RestAPICaller.CallApiGetParagraphs(location.Trim() + "-" + input.Trim() + "-" + typeInput.Trim());
            content = TextProcessing.PreprocessParagraphsContent(content);
            placeInformation.Content = content.Trim();

            Place place = GooglePlacesAPI.GetPlaceDetails(input);
            if (place != null)
            {
                placeInformation.Rating = place.Result.Rating;
            }

            string[] sentences = Regex.Split(placeInformation.Content, @"(?<=[\.!\?])\s+");
            placeInformation.Sentiment = Paralleldots.GetSentiment(JArray.FromObject(sentences));

            return placeInformation;

        }
    }
}