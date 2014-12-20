using CoreTweet;
using CoreTweet.Streaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TweetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        /*
         * This Code was Written by
         * 
         * Ernest John Ndungu
         * Please do not distribute in uncouth ways
         * 
         * Donate via M-Pesa to 0710148737
         * 
         * Update 1 for Shell/Bash/CMD
         * 
         * 
         * 
         */
        public static void Start()
        {
           
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                var session = OAuth.Authorize("CWfPQ6dvtFMCgrnslLvaxsexH", "GAV5MtpCpzWjuzE54sJR3xR5WVouuWJ1eAwrYeW7dngBNhbU6T");
                var res = System.Diagnostics.Process.Start(session.AuthorizeUri.AbsoluteUri);
                Console.WriteLine("Welcome to CommandTweet, Twitter for Nerds..");
                Console.WriteLine("Enter Your Twitter authentication Pin:");
                string pin = "";

                while (string.IsNullOrWhiteSpace(pin))
                {
                    pin = Console.ReadLine();
                }

                var tokens = OAuth.GetTokens(session, pin);

                foreach (var m in tokens.Streaming.StartStream(StreamingType.User))
                {

                    switch (m.Type)
                    {

                        case MessageType.Create:
                            var status = (m as StatusMessage).Status;

                            string tweettext = JsonConvert.SerializeObject(status);
                            var matches = Regex.Matches(tweettext, @"(chotman|steve|governor|kiwaa)");

                            if (matches.Count > 0)
                            {
                                Console.WriteLine("From Class Guys: -->  @{0}: {1}", status.User.ScreenName, status.Text);
                            }
                            else
                            {
                                Console.WriteLine("New Tweet: -->  @{0}: {1}", status.User.ScreenName, status.Text);
                            }

                            Console.WriteLine();
                            break;

                        case MessageType.DirectMesssage:

                            var message = JsonConvert.DeserializeObject<DirectMessage>(m.Json);
                            Console.WriteLine("New DM from " + message.Sender.Name + ": Message: " + message.Text);
                            break;

                        case MessageType.Disconnect:
                            Console.WriteLine("disconected");
                            break;


                        case MessageType.Event:
                            var stats = (m as CoreTweet.Streaming.EventMessage).Json;
                            Console.WriteLine(stats);
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Please get an internet connection \n By Ernest John (Cajuna)");
                Console.ReadKey();
            }
            
           
        }

    }
}
