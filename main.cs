using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication3
{
    internal static class Program
    {
        private static string Key { get; set; } = "";

        // private static string Format(string message)
        // {
        //     if (message == null) throw new ArgumentNullException(nameof(message));
        //     return message?.Replace("[", null)?.Replace("]", null)?.Replace("\"", null)?.Replace(" ", null);
        // }

        private static string IpAddress() =>
            JObject.Parse(new WebClient().DownloadString("https://api.ipify.org/?format=json"))["ip"]
                ?.ToString();

        private static string GetLiveExchange() => new WebClient().DownloadString("https://9hits.com/ajax.html?type=hits").Substring(8);

        private static string GetUsers() => new WebClient().DownloadString("https://9hits.com/ajax.html?type=users");

        private static string GetSites() => new WebClient().DownloadString("https://9hits.com/ajax.html?type=sites");

        public static void Main()
        {
            if (Key != null && Key == string.Empty)
            {
                Console.Write("Enter Key:");
                Key = Console.ReadLine();
            }

            using (var webClient = new WebClient())
            {
                var hitsData = JObject.Parse(webClient.DownloadString($"https://panel.9hits.com/api/profileGet?key={Key}"));

                switch (hitsData["status"]?.ToString())
                {
                    case "error":
                        Console.Write("Error ip not whitelisted/api closed/does not exist");
                        Console.ReadKey();
                        Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine($"IP:{IpAddress()}");
                        Console.WriteLine($"Live Exchange:{GetLiveExchange()}");
                        Console.WriteLine($"Users:{GetUsers()}");
                        Console.WriteLine($"Sites:{GetSites()}");
                        Console.WriteLine($"Username:{hitsData["data"]?["username"]}");
                        Console.WriteLine($"Email:{hitsData["data"]?["email"]}");
                        Console.WriteLine($"Joined:{hitsData["data"]?["joined"]}");
                        Console.WriteLine($"Token:{hitsData["data"]?["token"]}");
                        Console.WriteLine($"Funds:{hitsData["data"]?["funds"]}");
                        Console.WriteLine("Slots:");
                        Console.WriteLine($"    Used:{hitsData["data"]?["slots"]?["used"]}");
                        Console.WriteLine($"    Available:{hitsData["data"]?["slots"]?["available"]}");
                        Console.WriteLine($"Points:{hitsData["data"]?["points"]}");
                        Console.WriteLine($"Membership:{hitsData["data"]?["membership"]}");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
