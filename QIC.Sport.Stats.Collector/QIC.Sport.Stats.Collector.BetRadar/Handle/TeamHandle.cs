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
using QIC.Sport.Stats.Collector.Cache.CombinedData;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class TeamHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            TeamParam param = bd.Param as TeamParam;

            if (string.IsNullOrEmpty(bd.Html)) return;
            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  解析队伍基本信息
            TeamEntity te = new TeamEntity();
            te.TeamId = param.TeamId;
            te.Mark = "https://ls.betradar.com/ls/crest/big/" + param.TeamId + ".png";

            var teamInfo = cdata[2];
            var root = GetHtmlRoot(teamInfo);

            var teamName = root.SelectSingleNode("//thead/tr").InnerText;
            te.TeamName = teamName;

            var trsTeam = root.SelectNodes("//tbody/tr");
            if (trsTeam.Count > 2)
            {
                te.Manager = trsTeam[1].LastChild.InnerText;
                te.Venue = trsTeam[2].LastChild.InnerText;
                var teamEntity = TeamEntityManager.AddOrGetCacheEntity<TeamEntity>(te.TeamId);
                teamEntity.CompareSetTeamEntity(te);
            }

            #region  队伍球员相关信息

            //  解析进球数获得队员的点球个数，更新到队员点球信息缓存中
            var penGoals = cdata[14];
            root = GetHtmlRoot(penGoals);
            var trsPenGoals = root.SelectNodes("//tbody/tr");
            if (trsPenGoals != null)
            {
                foreach (var tr in trsPenGoals)
                {
                    var player = tr.SelectSingleNode("td[@class='player']");
                    if (player == null) continue;
                    var playerId = RegexGetStr(player.InnerHtml, "playerid', '", "',");
                    var pen = tr.LastChild.InnerText;
                    PlayerPenalties pp = PlayerPenaltiesManager.AddOrGetCacheEntity<PlayerPenalties>(playerId + "_" + param.SeasonId);
                    pp.PlayerId = playerId;
                    pp.SeasonId = param.SeasonId;
                    pp.ComparePlayerPenalties(pen);
                }
            }

            //  解析全部名单，并添加球员任务
            var playersData = cdata[16];
            root = GetHtmlRoot(playersData);
            var trsPlayer = root.SelectNodes("//tbody/tr");
            List<string> list = new List<string>();
            foreach (var tr in trsPlayer)
            {
                var s = tr.Attributes["onclick"].Value;
                var playerId = RegexGetStr(s, "playerid', '", "',");
                list.Add(playerId);
            }
            var tp = TeamPlayersManager.AddOrGetCacheEntity<TeamPlayers>(param.TeamId + "_" + param.SeasonId);
            var dic = tp.CompareSetPlayerIdList(list);
            #endregion
            NextAssignTask(param, dic);

            //  如果有添加获取伤停的任务
            if (txt.IndexOf("o=\"1003\"") > 0)
            {
                InjuryParam ip = param.CopyBaseParam<InjuryParam>();
                LeagueManager.AddOrUpdateParam(ip);
            }
        }

        //  分配任务
        private void NextAssignTask(TeamParam param, Dictionary<string, List<string>> taskDic)
        {
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        PlayerParam pp = param.CopyBaseParam<PlayerParam>();
                        pp.PlayerId = o;
                        PlayerManager.AddOrUpdateParam(pp);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        PlayerParam pp = param.CopyBaseParam<PlayerParam>();
                        pp.PlayerId = o;
                        PlayerManager.RemoveParam(pp);
                    });
                }
            }
        }
    }


}
