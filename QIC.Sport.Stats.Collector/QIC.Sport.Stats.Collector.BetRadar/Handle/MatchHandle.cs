using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class MatchHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            MatchParam param = bd.Param as MatchParam;
            if (string.IsNullOrEmpty(bd.Html)) return;
            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            var matchData = cdata[15];
            var root = GetHtmlRoot(matchData);

            bool isTitle = true;
            string round = "";
            foreach (var node in root.ChildNodes)
            {
                switch (node.Name)
                {
                    case "#text":
                        continue;
                    case "h2": if (isTitle) isTitle = false;
                        else round = node.InnerText;
                        continue;
                    case "table":
                        ParseMatch(round, node, param);
                        continue;
                }
            }
        }

        public void ParseMatch(string round, HtmlNode table, MatchParam param)
        {
            var trs = table.SelectNodes("tbody/tr");
            foreach (var tr in trs)
            {

                var matchDate = tr.SelectSingleNode("td[@class='datetime']").InnerText;
                var matchIdStr = tr.SelectSingleNode("td/a").Attributes["href"].Value;
                var homeIdStr = tr.SelectSingleNode("td/a/img[@class='home']").Attributes["src"].Value;
                var awayIdStr = tr.SelectSingleNode("td/a/img[@class='away']").Attributes["src"].Value;

                var htResult = tr.SelectSingleNode("td[@class='p1 ']").InnerText;
                var ftResult = tr.SelectSingleNode("td[@class='nt ftx ']").InnerText;

                var matchId = RegexGetStr(matchIdStr, "matchid', ", ",");
                var currentMatch = new MatchEntity();
                currentMatch.MatchId = matchId;
                currentMatch.HomeId = RegexGetStr(homeIdStr, "small/", ".png");
                currentMatch.AwayId = RegexGetStr(awayIdStr, "small/", ".png");
                currentMatch.MatchDate = matchDate;
                currentMatch.ExtendedData = "Round = " + round;
                currentMatch.SportId = param.SportId;
                currentMatch.SeasonId = param.SeasonId;

                var me = MatchEntityManager.AddOrGetCacheEntity<MatchEntity>(matchId);
                me.CompareSetMatchInfo(currentMatch);

                if (!string.IsNullOrEmpty(htResult) || !string.IsNullOrEmpty(ftResult))
                {
                    var result = new MatchResultEntity()
                    {
                        MatchId = matchId,
                        Result = htResult + "_" + ftResult,
                        SportId = param.SportId
                    };
                    me.CompareSetMatchResult(result);
                }
            }
        }
    }
}
