using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class BaseHandle
    {
        protected static readonly IWorkManager OrganizerManager = IocUnity.GetService<IWorkManager>(typeof(OrganizerManager).Name);
        protected static readonly IWorkManager LeagueManager = IocUnity.GetService<IWorkManager>(typeof(LeagueManager).Name);
        protected static readonly IWorkManager TeamManager = IocUnity.GetService<IWorkManager>(typeof(TeamManager).Name);
        protected static readonly IWorkManager PlayerManager = IocUnity.GetService<IWorkManager>(typeof(PlayerManager).Name);


        protected static readonly ICacheManager OrganizerEntityManager = IocUnity.GetService<ICacheManager>(typeof(OrganizerEntityManager).Name);
        protected static readonly ICacheManager LeagueEntityManager = IocUnity.GetService<ICacheManager>(typeof(LeagueEntityManager).Name);
        protected static readonly ICacheManager MatchEntityManager = IocUnity.GetService<ICacheManager>(typeof(MatchEntityManager).Name);
        protected static readonly ICacheManager PlayerEntityManager = IocUnity.GetService<ICacheManager>(typeof(PlayerEntityManager).Name);
        protected static readonly ICacheManager PlayerPenaltiesManager = IocUnity.GetService<ICacheManager>(typeof(PlayerPenaltiesManager).Name);
        protected static readonly ICacheManager PlayerStatisticsRecordManager = IocUnity.GetService<ICacheManager>(typeof(PlayerStatisticsRecordManager).Name);
        protected static readonly ICacheManager PlayerTimeRecordManager = IocUnity.GetService<ICacheManager>(typeof(PlayerTimeRecordManager).Name);
        protected static readonly ICacheManager SeasonEntityManager = IocUnity.GetService<ICacheManager>(typeof(SeasonEntityManager).Name);
        protected static readonly ICacheManager SeasonTeamsManager = IocUnity.GetService<ICacheManager>(typeof(SeasonTeamsManager).Name);
        protected static readonly ICacheManager SeasonTypeInfoManager = IocUnity.GetService<ICacheManager>(typeof(SeasonTypeInfoManager).Name);
        protected static readonly ICacheManager SeasonTableInfoManager = IocUnity.GetService<ICacheManager>(typeof(SeasonTableInfoManager).Name);
        protected static readonly ICacheManager TeamEntityManager = IocUnity.GetService<ICacheManager>(typeof(TeamEntityManager).Name);
        protected static readonly ICacheManager TeamPlayersManager = IocUnity.GetService<ICacheManager>(typeof(TeamPlayersManager).Name);

        public bool HtmlDecode(string html, out string txt)
        {
            txt = "";
            if (string.IsNullOrEmpty(html)) return false;
            if (html.Contains("error")) return false;
            txt = HttpUtility.HtmlDecode(html);
            return true;
        }

        public HtmlNode GetHtmlRoot(string html)
        {
            HtmlAgilityPack.HtmlDocument d = new HtmlAgilityPack.HtmlDocument();
            d.LoadHtml(html);
            var root = d.DocumentNode;
            return root;
        }

        public void CheckSetHistoryParam(BRBaseParam param)
        {
            if (param.IsHistoryParam)
                param.IsHistoryComplete = true;
        }
        public static string GetDataLikeKey(List<string> cdata, string likeKey, string notLikeKey = null)
        {
            string ret = null;
            foreach (var d in cdata)
            {
                if (d.Contains(likeKey))
                {
                    if (string.IsNullOrEmpty(notLikeKey))
                    {
                        ret = d;
                        break;
                    }
                    else if (!d.Contains(notLikeKey))
                    {
                        ret = d;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 正则获取指定字串之间的字符串
        /// </summary>
        /// <param name="originStr">完整的字符串</param>
        /// <param name="startStr">开始字串</param>
        /// <param name="endStr">结束字串</param>
        /// <returns>两个字串之间的字符串</returns>
        public static string RegexGetStr(string originStr, string startStr, string endStr)
        {
            Regex rg = new Regex("(?<=(" + startStr + "))[.\\s\\S]*?(?=(" + endStr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(originStr).Value;
        }

    }
}
