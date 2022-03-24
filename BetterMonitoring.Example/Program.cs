using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterMonitoring.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            API.BetterMonitoringClient client = new API.BetterMonitoringClient(your token);

            System.Net.WebHeaderCollection header = new System.Net.WebHeaderCollection
            {
                { "serverCount", "228" },
                { "shardCount", "1337" }
            };
            client.Refresh(header);

            Console.WriteLine("Successfully refreshed!");

            var bot = client.GetBot(952261877478608906);

            foreach (var property in bot.GetType().GetProperties())
            {
                if (property.Name == "Tags")
                {
                    Console.WriteLine(string.Format("{0}: {1}", property.Name, string.Join(", ", property.GetValue(bot) as IEnumerable<string>)));

                    continue;
                }
                else if (property.Name == "CoownersIds")
                {
                    Console.WriteLine(string.Format("{0}: {1}", property.Name, string.Join(",", (property.GetValue(bot) as IEnumerable<long>).Select(x => x.ToString()))));

                    continue;
                }

                Console.WriteLine(string.Format("{0}: {1}", property.Name, property.GetValue(bot)));
            }

            Console.ReadLine();
        }
    }
}
