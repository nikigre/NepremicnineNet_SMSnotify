using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Nepremicnine.netNotify
{
    class Program
    {
        static readonly string NepremecnineNetURL = "URL";
        static readonly string SMSsenderAPIkey = "KEY";
        static readonly int ThreadSleep = 1000 * 60 * 5; // ms * seconds * minutes = 5 min
        static readonly string[] PhoneNumbers = new string[] {
            "00386xxxxxxxx"
        };

        static Dictionary<string, string> ads = new Dictionary<string, string> { };

        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    //Download the page
                    string html;
                    using (WebClient client = new WebClient())
                    {
                        html = client.DownloadString(NepremecnineNetURL);
                    }

                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    //Get the list of ads
                    HtmlNode seznam = doc.DocumentNode.SelectNodes("//div[contains(@class, 'seznam')]")[0];
                    Dictionary<string, string> trenutno = new Dictionary<string, string> { };

                    foreach (var item in seznam.ChildNodes)
                    {
                        //If it is not an element we skip it
                        if (item.NodeType != HtmlNodeType.Element)
                            continue;

                        try
                        {
                            //If it does not containd an ID, then it is not an ad
                            if (!item.Attributes.Contains("id"))
                                continue;

                            string id = item.Attributes["id"].Value;
                            string url = "";

                            foreach (var item1 in item.ChildNodes)
                            {
                                if (item1.NodeType != HtmlNodeType.Comment)
                                    continue;

                                url = getURL(item1);
                                if (url.Contains("<"))
                                    goto label; //Yes. GOTO... hahaha
                                break;
                            }

                            trenutno.Add(id, url);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error while processing an ad. Exception:" + ex.Message);
                        }

                    label: Console.Write("");
                    }

                    if (ads.Count == 0)
                    {
                        ads = trenutno;
                        continue;
                    }

                    //We check if there is something new on the website
                    bool isEqual = Enumerable.SequenceEqual(trenutno.Keys.OrderBy(e => e), ads.Keys.OrderBy(e => e));

                    if (!isEqual)
                    {
                        //We go thorugh all new ads and send an SMS
                        foreach (var item in trenutno)
                        {
                            if (!ads.ContainsKey(item.Key))
                            {
                                SendSMS(item);
                                ads.Add(item.Key, item.Value);
                            }
                        }
                    }

                    Console.WriteLine(DateTime.Now + " Number of ads: " + ads.Count + " Curently read: " + trenutno.Count);
                    System.Threading.Thread.Sleep(ThreadSleep);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in the while loop. Exception:");
                Console.WriteLine(ex.ToString());
                Main(args);
            }
        }

        /// <summary>
        /// Returns the URL from the HTMLNode
        /// </summary>
        /// <param name="node">HTMLNode to extract an URL</param>
        /// <returns></returns>
        private static string getURL(HtmlNode node)
        {
            return node.InnerHtml.Replace("<!--<meta itemprop=\"url\" content=\"", "").Replace("\" />-->", "");
        }

        private static void SendSMS(KeyValuePair<string, string> trenutno)
        {
            string message = "New ad.\nID: " + trenutno.Key + "\nURL: " + trenutno.Value;
            Console.Write(DateTime.Now + "\n" + message + "\n");
            foreach (var item in PhoneNumbers)
            {
                if (SendSMS(item, message))
                {
                    Console.WriteLine("SMS sent to " + item + " succesfully!");
                }
                else
                {
                    Console.WriteLine("SMS sent to " + item + " unsuccesfully!");
                }
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Send an SMS to a desired phone number
        /// </summary>
        /// <param name="phone">Phone number</param>
        /// <param name="message">Message to send</param>
        /// <returns></returns>
        private static bool SendSMS(string phone, string message)
        {
            string h;
            using (WebClient client = new WebClient())
            {
                h = client.DownloadString("https://sms.nikigre.si/sendSMS?key=" + SMSsenderAPIkey + "&phone=" + phone + "&message=" + HttpUtility.UrlEncode(message));
            }

            if (h.Contains("\"OK\""))
                return true;

            return false;
        }
    }
}
