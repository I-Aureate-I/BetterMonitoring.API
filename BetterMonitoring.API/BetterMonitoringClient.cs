﻿using System;
using System.IO;
using System.Net;

namespace BetterMonitoring.API
{
    /// <summary>
    /// Gets a https://monitor.betterbot.ru/ client.
    /// </summary>
    public class BetterMonitoringClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetterMonitoringClient"/> class.
        /// </summary>
        /// <param name="token">Your bot token.</param>
        public BetterMonitoringClient(string token = "") => Token = token;

        /// <summary>
        /// Call request for get a bot data.
        /// </summary>
        /// <param name="url">Url for request.</param>
        /// <returns>Data of a bot.</returns>
        public string Request(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return result;
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[{0:MM/dd/yy H:mm:ss}] [{1}] [ERROR] {2}\n{3}", DateTime.Now.ToLocalTime().ToString(), nameof(Request), error.ToString(), error.StackTrace.ToString()));
                return null;
            }
        }

        /// <summary>
        /// Call request for change bot stats.
        /// </summary>
        /// <param name="url">Url for request.</param>
        /// <param name="headers">Headers which contains new stats.</param>
        /// <returns></returns>
        public bool Request(string url, WebHeaderCollection headers)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                headers.Add("Authorization", Token);
                request.Headers = headers;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return result.Split(':')[1] == "true" ? true : false;
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[{0:MM/dd/yy H:mm:ss}] [{1}] [ERROR] {2}\n{3}", DateTime.Now.ToLocalTime().ToString(), nameof(Request), error.ToString(), error.StackTrace.ToString()));
            }

            return false;
        }

        /// <summary>
        /// Call request for change bot stats.
        /// </summary>
        /// <param name="url">Url for request.</param>
        /// <param name="headers">Headers which contains new stats.</param>
        /// <returns></returns>
        public bool Request(string url, string[] headers)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                WebHeaderCollection header = new WebHeaderCollection
                {
                    { "Authorization", Token }
                };

                foreach (string headerValue in headers)
                    header.Add(headerValue);

                request.Headers = header;


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    return result.Split(':')[1] == "true" ? true : false;
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("[{0:MM/dd/yy H:mm:ss}] [{1}] [ERROR] {2}\n{3}", DateTime.Now.ToLocalTime().ToString(), nameof(Request), error.ToString(), error.StackTrace.ToString()));
            }

            return false;
        }

        /// <summary>
        /// Gets a <see cref="Bot"/> by <paramref name="botId"/>.
        /// </summary>
        /// <param name="botId">A id of bot.</param>
        /// <returns><see cref="Bot"/> which id equal to <paramref name="botId"/>.</returns>
        public Bot GetBot(long botId) => Bot.Get(Request(string.Format("{0}/bots/{1}", API_URL, botId)));

        /// <summary>
        /// Refresh bot stats.
        /// </summary>
        /// <param name="headers">New stats.</param>
        /// <returns>Successfully or not.</returns>
        public bool Refresh(WebHeaderCollection headers) => Request(string.Format("{0}/bots/stats", API_URL), headers);

        /// <summary>
        /// Refresh bot stats.
        /// </summary>
        /// <param name="headers">New stats.</param>
        /// <returns>Successfully or not.</returns>
        public bool Refresh(string[] headers) => Request(string.Format("{0}/bots/stats", API_URL), headers);

        /// <summary>
        /// Gets a token of your bot.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets a url adress of api.
        /// </summary>
        public const string API_URL = "https://monitor.betterbot.ru/api";
    }
}
