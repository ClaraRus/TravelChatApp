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

namespace TravelChatApp.DataOperations
{
	class TextProcessing
	{
        public static string[] DeserializeActivitiesAPI(string response)
        {
            string[] suggestions;
            response = Regex.Replace(response, "\"", "");
            response = response.Replace("[", "");
            response = Regex.Replace(response, "]", "");
            response = response.Replace('\\', ' ');

            int i = 0;
            suggestions = new string[response.Split(',').Length];

            foreach (string activity in response.Split(','))
            {
                if (activity != null && !activity.Equals(""))
                {
                    suggestions[i] = activity.Trim();
                    i++;
                }
            }

            return suggestions;
        }


        public static string PreprocessParagraphsContent(string content)
        {
            content = content.Replace(@"\n", "\n");
            content = content.Replace('\\', ' ');
            content = content.Replace('\"', ' ');

            string[] paragraphs = content.Split('\n');
            content = "";
            string temp;
            foreach (string paragraph in paragraphs)
            {
                if (paragraph.Length > 1)
                {
                    temp = paragraph;
                    temp = temp.Trim();
                    if (temp.First().ToString().Equals("."))
                        temp = temp.Substring(1);
                    if (!temp.Last().ToString().Equals("."))
                        temp += ".";

                    content += temp.First().ToString().ToUpper() + temp.Substring(1) + "\n\n";
                }
            }

            return content;
        }

    }
}