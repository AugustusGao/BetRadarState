using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class PlayerHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            PlayerParam param = bd.Param as PlayerParam;

            if (string.IsNullOrEmpty(bd.Html)) return;
            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  解析球员基本信息
            var playerInfo = cdata[1];
            var root = GetHtmlRoot(playerInfo);
            var tbody = root.SelectSingleNode("//div[@class='multiview_wrap']/table/tbody");
            var tds = tbody.SelectNodes("tr/td[@class='last ']");
            var tdCount = tds.Count;
            if (tdCount >= 10)
            {
                var currentPlayerEntity = new PlayerEntity()
                {
                    PlayerId = param.PlayerId,
                    FullName = tds[0].InnerText,
                    Country = tds[1].InnerText,
                    SecondaryCountry = tdCount == 11 ? tds[2].InnerText : "",
                    Birth = tds[tdCount - 8].InnerText,
                    Age = tds[tdCount - 7].InnerText,
                    Height = tds[tdCount - 6].InnerText,
                    Weight = tds[tdCount - 5].InnerText,
                    Position = tds[tdCount - 4].InnerText,
                    ShirtNumber = tds[tdCount - 3].InnerText,
                    TeamName = tds[tdCount - 2].InnerText,
                    PreferredFoot = tds[tdCount - 1].InnerText,
                };

                var pe = PlayerEntityManager.AddOrGetCacheEntity<PlayerEntity>(param.PlayerId);
                pe.CompareSetInfo(currentPlayerEntity);
            }
            else
            {
                //  todo 队员信息很少的情况
            }

            //  解析全部参赛记录
            //  记录比赛Id
            //  缓存出场信息PlayerTimeRecord
            //  缓存进球，牌，助攻信息PlayerStatisticsRecord
            var recordData = cdata[2];
            root = GetHtmlRoot(recordData);
            var tbodys = root.SelectNodes("//tbody");

            foreach (var tb in tbodys)
            {
                foreach (var node in tb.ChildNodes)
                {
                    var scoreStr = node.SelectSingleNode("td[@class='score']");
                    if (scoreStr == null || string.IsNullOrEmpty(scoreStr.InnerHtml)) continue;
                    var matchId = RegexGetStr(scoreStr.InnerHtml, "matchid','", "\'");
                    var nowPsr = new PlayerStatisticsRecord();
                    nowPsr.MatchId = matchId;
                    nowPsr.PlayerId = param.PlayerId;
                    nowPsr.Goals = Convert.ToInt32(node.SelectSingleNode("td[@class='goals']").InnerText);
                    var assists = node.SelectSingleNode("td[@class='assists']");
                    nowPsr.Assists = assists == null ? 0 : Convert.ToInt32(assists.InnerText);
                    nowPsr.YellowCard = Convert.ToInt32(node.SelectSingleNode("td[@class='yellow']").InnerText);
                    nowPsr.YellowRedCard = Convert.ToInt32(node.SelectSingleNode("td[@class='yellowred']").InnerText);
                    nowPsr.RedCard = Convert.ToInt32(node.SelectSingleNode("td[@class='red last']").InnerText);
                    var psr = PlayerStatisticsRecordManager.AddOrGetCacheEntity<PlayerStatisticsRecord>(param.PlayerId + "_" + matchId);
                    psr.CompareSet(nowPsr);

                    var nowPtr = new PlayerTimeRecord();
                    nowPtr.MatchId = matchId;
                    nowPtr.PlayerId = param.PlayerId;
                    nowPtr.IsStarting = Convert.ToInt32(node.SelectSingleNode("td[@class='started']").InnerText) == 1;
                    nowPtr.IsInPlay = Convert.ToInt32(node.SelectSingleNode("td[@class='in']").InnerText) == 1;
                    nowPtr.IsOutPlay = Convert.ToInt32(node.SelectSingleNode("td[@class='out']").InnerText) == 1;
                    nowPtr.MinutesPlayed = Convert.ToInt32(node.SelectSingleNode("td[@class='min']").InnerText);
                    var ptr = PlayerTimeRecordManager.AddOrGetCacheEntity<PlayerTimeRecord>(param.PlayerId + "_" + matchId);
                    ptr.CompareSet(nowPtr);
                }
            }


            //  如果有添加获取转会记录的任务
            if (txt.IndexOf("o=\"2\"") > 0)
            {
                PlayerTransferParam ptParam = param.CopyBaseParam<PlayerTransferParam>();
                PlayerManager.AddOrUpdateParam(ptParam);
            }
        }
    }
}
