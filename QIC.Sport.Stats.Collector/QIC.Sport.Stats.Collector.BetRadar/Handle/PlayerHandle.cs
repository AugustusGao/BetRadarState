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

            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  解析球员基本信息
            var playerInfo = cdata[1];
            var root = GetHtmlRoot(playerInfo);
            var tbody = root.SelectSingleNode("//div[@class='multiview_wrap']/table/tbody");

            Dictionary<string, string> infoDic = new Dictionary<string, string>();
            foreach (var node in tbody.ChildNodes)
            {
                infoDic.Add(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
            }
            var currentPlayerEntity = new PlayerEntity() { PlayerId = param.PlayerId };
            foreach (var info in infoDic)
            {
                switch (info.Key)
                {
                    case "全名:":
                    case "姓名:":
                    case "Full name:":
                    case "Name:":
                        currentPlayerEntity.FullName = info.Value;
                        break;
                    case "国家:":
                    case "Country:":
                        currentPlayerEntity.Country = info.Value;
                        break;
                    case "第二国籍:":
                    case "Secondary country:":
                        currentPlayerEntity.SecondaryCountry = info.Value;
                        break;
                    case "出生日期:":
                    case "Date of birth:":
                        currentPlayerEntity.Birth = info.Value;
                        break;
                    case "年龄:":
                    case "Age:":
                        currentPlayerEntity.Age = info.Value;
                        break;
                    case "身高:":
                    case "Height:":
                        currentPlayerEntity.Height = info.Value;
                        break;
                    case "体重:":
                    case "Weight:":
                        currentPlayerEntity.Weight = info.Value;
                        break;
                    case "位置:":
                    case "Position:":
                        currentPlayerEntity.Position = info.Value;
                        break;
                    case "球衣号码:":
                    case "Shirt number:":
                        currentPlayerEntity.ShirtNumber = info.Value;
                        break;
                    case "球队:":
                    case "Team:":
                        currentPlayerEntity.TeamName = info.Value;
                        break;
                    case "惯用脚:":
                    case "Preferred foot:":
                        currentPlayerEntity.PreferredFoot = info.Value;
                        break;
                }
            }
            var pe = PlayerEntityManager.AddOrGetCacheEntity<PlayerEntity>(param.PlayerId);
            pe.CompareSetInfo(currentPlayerEntity);

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
