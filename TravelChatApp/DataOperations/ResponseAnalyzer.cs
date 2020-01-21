using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TravelChatApp.Datastore;

namespace TravelChatApp.DataOperations
{

    class ResponseAnalyzer
    {
        
        public ResponseAnalyzer()
        {
            
        }

        public static string GetResponse(List<Review> reviews, Dictionary<List<string>, string> keywords)
        {
            List<Review> filteredReviews;
            filteredReviews= FilterReviews(reviews, keywords);

             string answer = ProcessFileredReviews(filteredReviews, keywords);

            return answer;
        }

        //ar trebui sa scot cuvintele de legatura
        private static List<Review> FilterReviews(List<Review> reviews, Dictionary<List<string>, string> keywords)
        {
            
            List<Review> filteredReviews = new List<Review>();
            foreach(List<string> key in keywords.Keys)
                    filteredReviews.AddRange(reviews.FindAll(review => review.Text.Contains(keywords[key])));

            if (filteredReviews.Count == 0)
                return reviews;

            return filteredReviews;
        }


        //iterate in the filtered reviews Texts and compose an answer from the prep that contain the keywords
        private static string ProcessFileredReviews(List<Review> filteredReviews, Dictionary<List<string>, string> keywords)
        {
            string[] sentences;
            string answer="";

            string allReviews="";
            foreach(Review review in filteredReviews)
            {
                sentences = review.Text.Split(".");
                foreach (string sentence in sentences)
                    foreach (List<string> key in keywords.Keys)
                        if (sentence.Contains(keywords[key]))
                            answer += sentence + ".";

                Console.WriteLine(review.Text);
                allReviews += review.Text;
            }

            if (answer.Equals(""))
                return "Sorry I could not find specific information.\n" +
                    "Here is what I could find:\n" + allReviews;

            return answer;
        }
    }
}