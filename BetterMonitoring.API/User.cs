using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterMonitoring.API
{
    /// <summary>
    /// A user of https://monitor.betterbot.ru/.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User(long id, string biography, string webSite, string github, string instagram, string twitter)
        {
            Id = id;
            Bio = biography is "\"," ? "Not specified" : biography;
            WebSite = webSite is "\"," ? "Not specified" : webSite;
            Github = github is "\"," ? "Not specified" : github;
            Instagram = instagram is "\"," ? "Not specified" : instagram;
            Twitter = twitter is "\"," ? "Not specified" : string.IsNullOrEmpty(twitter) ? "Not specified" : twitter;
        }

        /// <summary>
        /// Gets a user id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Gets a user biography.
        /// </summary>
        public string Bio { get; }

        /// <summary>
        /// Gets a user biography.
        /// </summary>
        public string WebSite { get; }

        /// <summary>
        /// Gets a user nickname on github.
        /// </summary>
        public string Github { get; }

        /// <summary>
        /// Gets a user nickname on instagram.
        /// </summary>
        public string Instagram { get; }

        /// <summary>
        /// Gets a user nickname on twitter.
        /// </summary>
        public string Twitter { get; }

        /// <summary>
        /// Gets a <see cref="User"/> and his data by <paramref name="data"/>.
        /// </summary>
        /// <param name="data">A user data</param>
        /// <returns><see cref="User"/> class which linked with <paramref name="data"/></returns>
        public static User Get(string data)
        {
            try
            {
                return new User(long.Parse(GetValue(data, "id")), GetValue(data, "biography"), GetValue(data, "website"), GetValue(data, "github"), GetValue(data, "twitter"), GetValue(data, "instagram"));
            }
            catch (Exception error)
            {
                Console.WriteLine(string.Format("[{0:MM/dd/yy H:mm:ss}] [{1}] [ERROR] {2}\n{3}", DateTime.Now.ToLocalTime().ToString(), nameof(Client.GetBot), error.ToString(), error.StackTrace.ToString()));
            }

            return null;
        }

        private static string GetValue(string data, string valueName) => Regex.Match(data, string.Format(@"\x22{0}\x22:\x22(.+?)\x22", valueName)).Groups[1].Value;
    }
}
