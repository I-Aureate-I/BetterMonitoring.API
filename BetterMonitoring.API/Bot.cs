using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetterMonitoring.API
{
    /// <summary>
    /// A bot which is contained in https://monitor.betterbot.ru/.
    /// </summary>
    public class Bot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        protected Bot(string userName, string prefix, string avatar, string id, string votes, string verified, string git, string web, string support, string ownerId, string shortDesc, string longDesc, string owner, string tags, string coowners)
        {
            UserName = userName;
            Prefix = prefix;
            Avatar = avatar;
            Id = long.Parse(id);
            Votes = short.Parse(votes);
            IsVerified = verified is "None" ? false : true;
            Github = string.IsNullOrEmpty(git) ? "None" : git == "\"," ? "None" : git;
            WebSite = string.IsNullOrEmpty(web) ? "None" : web == "\"," ? "None" : web;
            Support = string.IsNullOrEmpty(support) ? "None" : support == "\"," ? "None" : support;
            OwnerId = long.Parse(ownerId);
            ShortDesc = shortDesc;
            LongDesc = longDesc;
            Owner = owner;
            Tags = tags.Replace("\"", string.Empty).Split(',');

            if (coowners != "\"\"" && !coowners.Contains("]"))
            {
                CoownersIds = coowners.Replace("\"", string.Empty).Split(',').Select(x => long.Parse(x)).ToArray();
            }
            else
                CoownersIds = new long[0];
        }

        /// <summary>
        /// A bot username.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// A bot name.
        /// </summary>
        public string Name => UserName.Split('#')[0];

        /// <summary>
        /// A bot discriminator.
        /// </summary>
        public short Discriminator => short.Parse(UserName.Split('#')[1]);

        /// <summary>
        /// Gets a bot prefix of commands.
        /// </summary>
        public string Prefix { get; }

        /// <summary>
        /// Gets a bot tags.
        /// </summary>
        public string[] Tags { get; }

        /// <summary>
        /// Gets a bot short desc.
        /// </summary>
        public string ShortDesc { get; }

        /// <summary>
        /// Gets a bot long desc.
        /// </summary>
        public string LongDesc { get; }

        /// <summary>
        /// Gets a url to bot avatar.
        /// </summary>
        public string Avatar { get; }

        /// <summary>
        /// Gets a bot id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Gets a bot owner name.
        /// </summary>
        public string Owner { get; }

        /// <summary>
        /// Gets a bot owner id.
        /// </summary>
        public long OwnerId { get; }

        /// <summary>
        /// Gets a bot coowners ids.
        /// </summary>
        public long[] CoownersIds { get; }

        /// <summary>
        /// Gets a votes count of bot.
        /// </summary>
        public short Votes { get; }

        /// <summary>
        /// Gets a value which indicating certified bot or not.
        /// </summary>
        public bool IsVerified { get; }

        /// <summary>
        /// Gets a github page of bot.
        /// </summary>
        public string Github { get; }

        /// <summary>
        /// Gets a website of bot.
        /// </summary>
        public string WebSite { get; }

        /// <summary>
        /// Gets a url to bot support.
        /// </summary>
        public string Support { get; }

        /// <summary>
        /// Gets a <see cref="Bot"/> and his data by <paramref name="data"/>.
        /// </summary>
        /// <param name="data">A bot data</param>
        /// <returns><see cref="Bot"/> class which linked with <paramref name="data"/></returns>
        public static Bot Get(string data)
        {
            try
            {
                return new Bot(
                        GetValue(data, "username") + "#" + GetValue(data, "discrim"),
                        GetValue(data, "prefix"),
                        GetValue(data, "avatar"),
                        GetValue(data, "botID"),
                        GetValue(data),
                        GetValue(data, "certificate"),
                        GetValue(data, "github"),
                        GetValue(data, "website"),
                        GetValue(data, "support"),
                        GetValue(data, "ownerID"),
                        GetValue(data, "shortDesc"),
                        GetValue(data, "longDesc"),
                        GetValue(data, "owner"),
                        GetValues(data, "tags"),
                        GetValues(data, "coowners"));
            }
            catch (Exception error)
            {
                Console.WriteLine(string.Format("[{0:MM/dd/yy H:mm:ss}] [{1}] [ERROR] {2}\n{3}", DateTime.Now.ToLocalTime().ToString(), nameof(Client.GetBot), error.ToString(), error.StackTrace.ToString()));
            }

            return null;
        }

        private static string GetValue(string data) => Regex.Match(data, @"\x22votes\x22:(.+?)").Groups[1].Value;

        private static string GetValue(string data, string valueName) => Regex.Match(data, string.Format(@"\x22{0}\x22:\x22(.+?)\x22", valueName)).Groups[1].Value;

        private static string GetValues(string data, string valueName) => Regex.Match(data, string.Format(@"\x22{0}\x22:\[(.+?)\]", valueName)).Groups[1].Value;
    }
}
