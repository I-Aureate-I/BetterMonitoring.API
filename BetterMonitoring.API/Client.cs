using System;
using System.IO;
using System.Net;

namespace BetterMonitoring.API
{
    /// <summary>
    /// Gets a https://monitor.betterbot.ru/ client.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="token">Your bot token.</param>
        public Client(string token = "") => Token = token;

        /// <summary>
        /// Call request for get a bot data.
        /// </summary>
        /// <param name="url">Url for request.</param>
        /// <returns>Data of a bot.</returns>
        internal string Request(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
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
        /// <returns>Successfully or not.</returns>
        internal bool Request(string url, WebHeaderCollection headers)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                headers.Add("Authorization", Token);
                request.Headers = headers;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd().Split(':')[1] == "true";
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
        /// <returns>Successfully or not.</returns>
        internal bool Request(string url, string[] headers)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

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
                    return reader.ReadToEnd().Split(':')[1] == "true";
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
        /// Check user vote for bot.
        /// </summary>
        /// <param name="userId">A user id.</param>
        /// <returns>Value which indicating voted user for bot or not.</returns>
        public bool CheckVote(string userId)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bots/check/{1}", API_URL, userId));
                request.Method = "GET";
                WebHeaderCollection headers = new WebHeaderCollection
                {
                    { "Authorization", Token }
                };
                request.Headers = headers;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd().Split(':')[1].Replace("}", null) == "true";
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
