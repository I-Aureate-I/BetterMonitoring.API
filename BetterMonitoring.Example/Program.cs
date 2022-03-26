﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using BetterMonitoring.API;

namespace BetterMonitoring.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("your token");

            Console.WriteLine(client.CheckVote("950866731562311740"));

            WebHeaderCollection header = new WebHeaderCollection
            {
                { "serverCount", "228" },
                { "shardCount", "1337" }
            };
            Console.WriteLine(client.Refresh(header));

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
