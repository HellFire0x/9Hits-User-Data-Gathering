using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication3
{
    internal static class Program
    {
        /// <summary>
        ///     Key Data
        /// </summary>
        private static string Key { get; set; } = "";

        /// <summary>
        ///     Links Data
        /// </summary>
        private static string[] Links { get; } =
        {
            "ftp.9hits.com",
            "f.9hits.com",
            "forum.9hits.com",
            "mail.9hits.com",
            "www.9hits.com"
        };

        /// <summary>
        ///     Format Message/data
        /// </summary>
        /// <param name="message">Where the message goes to format</param>
        /// <returns>Return the formatted data message</returns>
        /// <exception cref="ArgumentNullException">If Empty or null</exception>
        private static string Format(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            return message.Replace("[", null).Replace("]", null).Replace("\"", null);
        }

        /// <summary>
        ///     Get IP Address
        /// </summary>
        /// <returns>Your public ip address</returns>
        private static string IpAddress()
        {
            return JObject.Parse(new WebClient().DownloadString("https://api.ipify.org/?format=json"))["ip"]
                ?.ToString();
        }

        /// <summary>
        ///     Get Live Exchange
        /// </summary>
        /// <returns>Hits Data the live currency points</returns>
        private static string GetLiveExchange()
        {
            return new WebClient().DownloadString("https://9hits.com/ajax.html?type=hits").Substring(8);
        }

        /// <summary>
        ///     Get Users Data
        /// </summary>
        /// <returns>User Data how many user are using the website</returns>
        private static string GetUsers()
        {
            return new WebClient().DownloadString("https://9hits.com/ajax.html?type=users");
        }

        /// <summary>
        ///     Get Sites Data
        /// </summary>
        /// <returns>How many sites are on this website</returns>
        private static string GetSites()
        {
            return new WebClient().DownloadString("https://9hits.com/ajax.html?type=sites");
        }

        /// <summary>
        ///     this is where the stuff starts the interesting stuff :)
        /// </summary>
        public static void Main()
        {
            if (Key != null && Key == string.Empty)
            {
                Console.Write("Enter Key:");
                Key = Console.ReadLine();
            }

            using (var webClient = new WebClient())
            {
                var hitsData =
                    JObject.Parse(webClient.DownloadString($"https://panel.9hits.com/api/profileGet?key={Key}"));

                switch (hitsData["status"]?.ToString())
                {
                    case "error":
                    {
                        Console.Write("Error ip not whitelisted/api closed/does not exist");
                        Console.ReadKey();
                        Environment.Exit(1);
                        break;
                    }
                    default:
                    {
                        Console.WriteLine($"IP: {IpAddress()}");
                        Console.WriteLine($"Live Exchange: {long.Parse(GetLiveExchange()):n}");
                        Console.WriteLine($"Users: {long.Parse(GetUsers()):n}");
                        Console.WriteLine($"Sites: {long.Parse(GetSites()):n}");
                        Console.WriteLine($"Username: {hitsData["data"]?["username"]}");
                        Console.WriteLine($"Email: {hitsData["data"]?["email"]}");
                        Console.WriteLine($"Joined: {hitsData["data"]?["joined"]}");
                        Console.WriteLine($"Token: {hitsData["data"]?["token"]}");
                        Console.WriteLine($"Funds: {hitsData["data"]?["funds"]}");
                        Console.WriteLine("Slots:");
                        Console.WriteLine($"    [+] - Used: {hitsData["data"]?["slots"]?["used"]}");
                        Console.WriteLine($"    [+] - Available: {hitsData["data"]?["slots"]?["available"]}");
                        Console.WriteLine($"Points: {hitsData["data"]?["points"]}");
                        Console.WriteLine($"Membership: {hitsData["data"]?["membership"]}");
                        Console.WriteLine(hitsData["messages"]?.ToString() == string.Empty
                            ? "Message: No messages"
                            : $"Message: {Format(hitsData["messages"]?.ToString())}");
                        Console.WriteLine("Links: ");
                        foreach (var links in Links)
                        {
                            Console.WriteLine($"    [+] + {links}");
                        }
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }
    }
}
