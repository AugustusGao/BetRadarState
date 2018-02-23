using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ML.Infrastructure.IOC;
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
            var roundDatas = root.SelectNodes("//table");

            int round = 1;
            foreach (var roundData in roundDatas)
            {
                var trs = roundData.SelectNodes("tbody/tr");
                foreach (var tr in trs)
                {

                    var matchDate = tr.SelectSingleNode("td[@class='datetime']").InnerText;
                    var matchIdStr = tr.SelectSingleNode("td/a").Attributes["href"].Value;
                    var homeIdStr = tr.SelectSingleNode("td/a/img[@class='home']").Attributes["src"].Value;
                    var awayIdStr = tr.SelectSingleNode("td/a/img[@class='away']").Attributes["src"].Value;

                    var htResult = tr.SelectSingleNode("td[@class='p1 ']").InnerText;
                    var ftResult = tr.SelectSingleNode("td[@class='nt ftx ']").InnerText;

                    var matchId = RegGetStr(matchIdStr, "matchid', ", ",");
                    var meCache = IocUnity.GetService<ICacheManager>(typeof(MatchEntityManager).Name);
                    var me = meCache.AddOrGetCacheEntity<MatchEntity>(matchId);
                    me.MatchId = matchId;
                    me.HomeId = RegGetStr(homeIdStr, "small/", ".png");
                    me.AwayId = RegGetStr(awayIdStr, "small/", ".png");
                    me.MatchDate = matchDate;
                    me.ExtendedData = "Round = " + round;
                    me.SportId = param.SportId;
                    me.SeasonId = param.SeasonId;

                    if (!string.IsNullOrEmpty(htResult) || !string.IsNullOrEmpty(ftResult))
                    {
                        var result = new MatchResultEntity()
                        {
                            MatchId = matchId,
                            Result = htResult + "_" + ftResult,
                            SportId = param.SportId
                        };
                        me.MatchResult = result;
                    }
                }
                round++;
            }
        }
    }
}
